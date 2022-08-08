using System;
using System.Threading.Tasks;
using DG.Tweening;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Utils;


namespace Infrastructure {

	internal class LoginPanelController : IDisposable {
		private const int SUCCESSFUL_STATUS_DELAY = 500;
		private const int LOADING_UPDATE_DELAY = 250;

		public event Action<string> OnUserLoginSuccess;
		
		private readonly LoginPanelView _loginPanelView;
		private bool _isNewAccount;
		private bool _isFading;
		private bool _emailFieldIsFading;
		private bool _isLoading;
		
		private string _username;
		private string _password;
		private string _email;


		public LoginPanelController(LoginPanelView loginPanelView) {
			_loginPanelView = loginPanelView;
		}

		public void Dispose() {
			_loginPanelView.CanvasGroup.DOKill();
			_loginPanelView.ConfirmButton.onClick.RemoveAllListeners();
			_loginPanelView.ClosePanelButton.onClick.RemoveAllListeners();
			_loginPanelView.UsernameInputField.onValueChanged.RemoveAllListeners();
			_loginPanelView.PasswordInputField.onValueChanged.RemoveAllListeners();
			_loginPanelView.EmailInputField.onValueChanged.RemoveAllListeners();
			_loginPanelView.EmailToggleSwitcher.onValueChanged.RemoveAllListeners();
		}

		public void Init() {
			InitButtons();
			InitEmailToggleSwitcher();
			InitPlayFabFields();


			void InitButtons() {
				_loginPanelView.ConfirmButtonText.text = TextConstants.LOGIN_PANEL_CONFIRM_BUTTON_LOGIN_ACCOUNT_TEXT;
				_loginPanelView.ConfirmButton.onClick.AddListener(ConfirmLogin);
				_loginPanelView.ClosePanelButton.onClick.AddListener(HidePanel);
				SetConfirmButtonInteractivity();
			}

			void InitEmailToggleSwitcher() {
				_loginPanelView.EmailToggleSwitcher.isOn = false;
				HideEmailField(false);
				_loginPanelView.EmailToggleSwitcherLabelCanvasGroup.gameObject.SetActive(true);
				_loginPanelView.EmailToggleSwitcherLabelCanvasGroup.alpha = 1;
				_loginPanelView.EmailToggleSwitcher.onValueChanged.AddListener(ToggleLoginMode);
			}

			void InitPlayFabFields() {
				if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)) {
					PlayFabSettings.staticSettings.TitleId = TextConstants.PLAYFAB_TITLE_ID;
				}
				_loginPanelView.StatusLableText.text = string.Empty;
				_loginPanelView.UsernameInputField.onValueChanged.AddListener(UpdateUsername);
				_loginPanelView.PasswordInputField.onValueChanged.AddListener(UpdatePassword);
				_loginPanelView.EmailInputField.onValueChanged.AddListener(UpdateEmail);
			}

			void UpdateUsername(string username) {
				_username = username;
				SetConfirmButtonInteractivity();
			}

			void UpdatePassword(string password) {
				_password = password;
				SetConfirmButtonInteractivity();
			}

			void UpdateEmail(string email) {
				_email = email;
				SetConfirmButtonInteractivity();
			}
		}

		public void ShowPanel() {
			if (_isFading) {
				_loginPanelView.CanvasGroup.DOKill();
			}
			_isFading = true;
			_loginPanelView.gameObject.SetActive(true);
			_loginPanelView.CanvasGroup.alpha = 0;
			_loginPanelView.CanvasGroup.DOFade(1, Constants.LOGIN_PANEL_FADING_DURATION)
					.OnComplete(() => {
						_isFading = false;
					});
		}

		private void HidePanel() {
			if (_isFading) {
				_loginPanelView.CanvasGroup.DOKill();
			}
			_isFading = true;
			_loginPanelView.CanvasGroup.DOFade(0, Constants.LOGIN_PANEL_FADING_DURATION)
					.OnComplete(() => {
						_loginPanelView.gameObject.SetActive(false);
						_isFading = false;
					});
		}

		/// <summary>
		/// Переключение между процессом входа в существующий аккаунт и созданием нового
		/// </summary>
		private void ToggleLoginMode(bool isNewAccount) {
			if (_emailFieldIsFading)
				return;

			_emailFieldIsFading = true;
			_isNewAccount = isNewAccount;
			SetConfirmButtonInteractivity();
			if (isNewAccount) {
				_loginPanelView.ConfirmButtonText.text = TextConstants.LOGIN_PANEL_CONFIRM_BUTTON_CREATE_ACCOUNT_TEXT;
				_loginPanelView.EmailToggleSwitcherLabelCanvasGroup.DOFade(0, Constants.LOGIN_PANEL_FADING_DURATION)
						.OnComplete(() => {
							_loginPanelView.EmailToggleSwitcherLabelCanvasGroup.gameObject.SetActive(false);
							ShowEmailField();
						});
			} else {
				_loginPanelView.ConfirmButtonText.text = TextConstants.LOGIN_PANEL_CONFIRM_BUTTON_LOGIN_ACCOUNT_TEXT;
				HideEmailField(onCompleteCallback: () => {
					_loginPanelView.EmailToggleSwitcherLabelCanvasGroup.gameObject.SetActive(true);
					_loginPanelView.EmailToggleSwitcherLabelCanvasGroup.DOFade(1, Constants.LOGIN_PANEL_FADING_DURATION);
					_emailFieldIsFading = false;
				});
			}

			
			void ShowEmailField() {
				_loginPanelView.EmailInputFieldCanvasGroup.gameObject.SetActive(true);
				_loginPanelView.EmailInputFieldCanvasGroup.DOFade(1, Constants.LOGIN_PANEL_FADING_DURATION)
						.OnComplete(() => {
							_emailFieldIsFading = false;
						});
			}
		}

		private void HideEmailField(bool animationIsOn = true, Action onCompleteCallback = null) {
			if (animationIsOn)
				_loginPanelView.EmailInputFieldCanvasGroup.DOFade(0, Constants.LOGIN_PANEL_FADING_DURATION)
						.OnComplete(() => {
							_loginPanelView.EmailInputFieldCanvasGroup.gameObject.SetActive(false);
							onCompleteCallback?.Invoke();
						});
			else {
				_loginPanelView.EmailInputFieldCanvasGroup.gameObject.SetActive(false);
				onCompleteCallback?.Invoke();
			}
		}

		private void SetConfirmButtonInteractivity() {
			_loginPanelView.ConfirmButton.interactable = !string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password) 
					&& (_isNewAccount && !string.IsNullOrEmpty(_email) || !_isNewAccount);
		}

		private void ConfirmLogin() {
			if (_isNewAccount)
				CreateAccount();
			else
				LoginAccount();
		}

		private void CreateAccount() {
			ToggleElementsInteractivity(false);
			_isLoading = true;
			ShowLoadingStatusAsync();
			PlayFabClientAPI.RegisterPlayFabUser(
					new RegisterPlayFabUserRequest {Username = _username, Email = _email, Password = _password, RequireBothUsernameAndEmail = true}
					, OnRegistrationSuccess
					, OnRegistrationFailure
			);

			
			void OnRegistrationSuccess(RegisterPlayFabUserResult registerPlayFabUserResult) {
				Debug.Log($"Registration success: {_username}");
				LoginAccount(true);
			}

			void OnRegistrationFailure(PlayFabError playFabError) {
				ToggleElementsInteractivity(true);
				_isLoading = false;
				_loginPanelView.StatusLableText.color = Color.red;
				_loginPanelView.StatusLableText.text = TextConstants.REGISTRATION_FAIL_STATUS_TEXT;
				Debug.LogError($"Registration fail: {playFabError.ErrorMessage}");
			}
		}

		private void LoginAccount(bool isAfterRegistrationLogin = false) {
			if (!isAfterRegistrationLogin) {
				ToggleElementsInteractivity(false);
				_isLoading = true;
				ShowLoadingStatusAsync();
			}
			PlayFabClientAPI.LoginWithPlayFab(
					new LoginWithPlayFabRequest { Username = _username, Password = _password }
					, OnLoginSuccessAsync
					, OnLoginFailure
			);


			async void OnLoginSuccessAsync(LoginResult loginPlayFabUserResult) {
				_isLoading = false;
				_loginPanelView.StatusLableText.color = Color.green;
				_loginPanelView.StatusLableText.text = TextConstants.LOGIN_SUCCESS_STATUS_TEXT;
				Debug.Log($"Login success: {_username}");
				await Task.Delay(SUCCESSFUL_STATUS_DELAY);
				OnUserLoginSuccess?.Invoke(_username);
				HidePanel();
			}

			void OnLoginFailure(PlayFabError playFabError) {
				ToggleElementsInteractivity(true);
				_isLoading = false;
				if (isAfterRegistrationLogin) {
					_loginPanelView.StatusLableText.color = Color.green;
					_loginPanelView.StatusLableText.text = TextConstants.REGISTRATION_SUCCESS_STATUS_TEXT;
				} else {
					_loginPanelView.StatusLableText.color = Color.red;
					_loginPanelView.StatusLableText.text = TextConstants.LOGIN_FAIL_STATUS_TEXT;
				}
				Debug.LogError($"Login fail: {playFabError.ErrorMessage}");
			}
		}

		private void ToggleElementsInteractivity(bool isInteratable) {
			_loginPanelView.UsernameInputField.interactable = isInteratable;
			_loginPanelView.PasswordInputField.interactable = isInteratable;
			_loginPanelView.EmailInputField.interactable = isInteratable;
			_loginPanelView.EmailToggleSwitcher.interactable = isInteratable;
			_loginPanelView.ConfirmButton.interactable = isInteratable;
			_loginPanelView.ClosePanelButton.interactable = isInteratable;
		}

		private async void ShowLoadingStatusAsync() {
			const int startAwaitingTick = 0;
			const int maxAwaitingTick = 5;
			_loginPanelView.StatusLableText.color = Color.cyan;
			var currentAwaitingTick = startAwaitingTick;
			while (_isLoading) {
				_loginPanelView.StatusLableText.text = $"{TextConstants.LOADING_STATUS_TEXT_TEMPLATE}{new string('.', currentAwaitingTick)}";
				if (++currentAwaitingTick >= maxAwaitingTick)
					currentAwaitingTick = startAwaitingTick;
				await Task.Delay(LOADING_UPDATE_DELAY);
			}
		}

	}

}