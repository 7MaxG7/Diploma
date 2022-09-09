using System;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Utils;
// ReSharper disable InconsistentNaming


namespace Infrastructure {

	internal class LoginPanelController : IDisposable {
		public event Action<string, string> OnUserLoginSuccess;
		
		private readonly LoginPanelView _loginPanelView;
		private bool _isNewAccount;
		private bool _isFading;
		private bool _emailFieldIsFading;
		private bool _isLoading;
		
		private string _username;
		private string _password;
		private string _email;
		private MainMenuConfig _mainMenuConfig;


		public LoginPanelController(LoginPanelView loginPanelView, MainMenuConfig mainMenuConfig) {
			_loginPanelView = loginPanelView;
			_mainMenuConfig = mainMenuConfig;
		}

		public void Dispose() {
			_loginPanelView.OnDispose();
			_loginPanelView.OnLoginConfirmClick -= ConfirmLogin;
			_loginPanelView.OnHideLoginPanelClick -= _loginPanelView.Hide;
			_loginPanelView.OnLoginModeSwitched -= ToggleLoginMode;
		}

		public void Init() {
			_loginPanelView.Init(_mainMenuConfig);
			_loginPanelView.OnLoginConfirmClick += ConfirmLogin;
			_loginPanelView.OnHideLoginPanelClick += _loginPanelView.Hide;
			_loginPanelView.OnLoginModeSwitched += ToggleLoginMode;
			if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)) {
				PlayFabSettings.staticSettings.TitleId = TextConstants.PLAYFAB_TITLE_ID;
			}
		}

		public void ShowPanel() {
			_loginPanelView.Show();
		}

		/// <summary>
		/// Switching between login to existing account and creating account modes
		/// </summary>
		private void ToggleLoginMode(bool isNewAccount) {
			_isNewAccount = isNewAccount;
		}

		private void ConfirmLogin(string userName, string password, string email) {
			_username = userName;
			_password = password;
			_email = email;
			if (_isNewAccount)
				CreateAccount();
			else
				LoginAccount();
		}

		private void CreateAccount() {
			_loginPanelView.ToggleElementsInteractivity(false);
			_loginPanelView.ShowLoadingStatusAsync();
			PlayFabClientAPI.RegisterPlayFabUser(
					new RegisterPlayFabUserRequest { Username = _username, Email = _email, Password = _password, RequireBothUsernameAndEmail = true }
					, OnRegistrationSuccess
					, OnRegistrationFailure
			);

			
			void OnRegistrationSuccess(RegisterPlayFabUserResult registerPlayFabUserResult) {
				Debug.Log($"Registration success: {_username}");
				LoginAccount(true);
			}

			void OnRegistrationFailure(PlayFabError playFabError) {
				_loginPanelView.ToggleElementsInteractivity(true);
				_loginPanelView.ShowRegistrationErrorStatus();
				Debug.LogError($"Registration fail: {playFabError.ErrorMessage}");
			}
		}

		private void LoginAccount(bool isAfterRegistrationLogin = false) {
			if (!isAfterRegistrationLogin) {
				_loginPanelView.ToggleElementsInteractivity(false);
				_loginPanelView.ShowLoadingStatusAsync();
			}
			PlayFabClientAPI.LoginWithPlayFab(
					new LoginWithPlayFabRequest { Username = _username, Password = _password }
					, OnLoginSuccessAsync
					, OnLoginFailure
			);


			async void OnLoginSuccessAsync(LoginResult loginPlayFabUserResult) {
				_loginPanelView.ShowLoginSuccessStatus();
				await Task.Delay(_mainMenuConfig.SuccessfulStatusDelay);
				OnUserLoginSuccess?.Invoke(_username, loginPlayFabUserResult.PlayFabId);
				_loginPanelView.Hide();
				Debug.Log($"Login success: {_username}");
			}

			void OnLoginFailure(PlayFabError playFabError) {
				_loginPanelView.ToggleElementsInteractivity(true);
				if (isAfterRegistrationLogin) {
					_loginPanelView.ShowRegistrationSuccessStatus();
				} else {
					_loginPanelView.ShowLoginErrorStatus();
				}
				Debug.LogError($"Login fail: {playFabError.ErrorMessage}");
			}
		}
	}

}