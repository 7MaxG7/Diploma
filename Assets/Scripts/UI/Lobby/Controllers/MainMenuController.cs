using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class MainMenuController : IMainMenuController {
		public event Action<string> OnPlayfabLogin;
		
		private string _userName;
		private string _playfabId;
		private MainMenuView _mainMenuView;
		private LoginPanelController _loginPanelController;
		private LobbyScreenController _lobbyScreenController;
		private readonly IPermanentUiController _permanentUiController;
		private readonly IMissionResultController _missionResultController;
		private readonly MainMenuConfig _mainMenuConfig;


		[Inject]
		public MainMenuController(IMissionResultController missionResultController, MainMenuConfig mainMenuConfig
				, IPermanentUiController permanentUiController) {
			_missionResultController = missionResultController;
			_mainMenuConfig = mainMenuConfig;
			_permanentUiController = permanentUiController;
		}

		public void Dispose() {
			_mainMenuView.LoginPanelButton.onClick.RemoveAllListeners();
			_mainMenuView.PlayButton.onClick.RemoveAllListeners();
			_mainMenuView.SettingsButton.onClick.RemoveAllListeners();
			_mainMenuView.CreditsButton.onClick.RemoveAllListeners();
			_mainMenuView.QuitGameButton.onClick.RemoveAllListeners();
			_mainMenuView.CreditsView.CloseCreditsButton.onClick.RemoveAllListeners();
			_loginPanelController.OnUserLoginSuccess -= SetUser;
			_loginPanelController.Dispose();
			_lobbyScreenController.Dispose();
			Object.Destroy(_mainMenuView.GameObject);
		}


		public void SetupMainMenu() {
			if (_mainMenuView == null) {
				_mainMenuView = Object.Instantiate(_mainMenuConfig.MainMenuPref);
			}
			_mainMenuView.GameObject.SetActive(true);
			_mainMenuView.HeaderLabel.text = TextConstants.MAIN_MENU_HEADER_TEXT;
			InitButtons();
			InitLoginPanel();
			InitLobbyPanel();
			UpdateButtonsInteractivity();
			SetupScoresLable();


			void InitButtons() {
				_mainMenuView.LoginPanelButton.onClick.AddListener(OpenLoginPanel);
				_mainMenuView.PlayButton.onClick.AddListener(OpenPlayPanel);
				_mainMenuView.SettingsButton.onClick.AddListener(OpenSettingsPanel);
				_mainMenuView.CreditsButton.onClick.AddListener(ShowCredits);
				_mainMenuView.QuitGameButton.onClick.AddListener(QuitGame);
				_mainMenuView.CreditsView.CloseCreditsButton.onClick.AddListener(HideCredits);
			}

			void InitLoginPanel() {
				_loginPanelController = new LoginPanelController(_mainMenuView.LoginPanelView);
				_loginPanelController.Init();
				_loginPanelController.OnUserLoginSuccess += SetUser;
			}

			void InitLobbyPanel() {
				_lobbyScreenController = new LobbyScreenController(_mainMenuView.LobbyScreenView, _mainMenuConfig, _permanentUiController);
				if (PlayFabClientAPI.IsClientLoggedIn()) {
					_lobbyScreenController.Init(_userName);
				}
			}
		}

		private void OpenLoginPanel() {
			_loginPanelController.ShowPanel();
		}

		private void OpenPlayPanel() {
			_lobbyScreenController.ShowScreen();
		}

		private void OpenSettingsPanel() {
			_permanentUiController.ShowSettingsPanel();
		}

		private void ShowCredits() {
			PrepareCredits();
			_mainMenuView.CreditsView.CanvasGroup.DOFade(1, _mainMenuConfig.CreditsFadingDuration)
					.OnComplete(() => {
						_mainMenuView.CreditsView.StartCoroutine(ScrollCredits());
					});
			

			void PrepareCredits() {
				_mainMenuView.CreditsView.GameObject.SetActive(true);
				_mainMenuView.CreditsView.CanvasGroup.alpha = 0;
				_mainMenuView.CreditsView.CreditsScroll.content.anchoredPosition = Vector2.zero;
				_mainMenuView.CreditsView.CanvasGroup.interactable = true;
			}

			IEnumerator ScrollCredits() {
				var endContentYPosition = _mainMenuView.CreditsView.CreditsScroll.content.rect.height -
				                          _mainMenuView.CreditsView.CreditsScroll.viewport.rect.height;
				var creditsScrollDelta = new Vector2(0, _mainMenuConfig.CreditsScrollSpeed);
				while (_mainMenuView.CreditsView.CreditsScroll.content.anchoredPosition.y < endContentYPosition) {
					_mainMenuView.CreditsView.CreditsScroll.content.anchoredPosition += creditsScrollDelta * Time.deltaTime;
					yield return new WaitForEndOfFrame();
				}

				HideCredits();
			}
		}

		private void HideCredits() {
			_mainMenuView.CreditsView.CanvasGroup.interactable = false;
			_mainMenuView.CreditsView.CanvasGroup.DOKill();
			_mainMenuView.CreditsView.StopAllCoroutines();
			_mainMenuView.CreditsView.CanvasGroup.DOFade(0, _mainMenuConfig.CreditsFadingDuration)
					.OnComplete(() => {
						_mainMenuView.CreditsView.GameObject.SetActive(false);
					});
		}

		private void QuitGame() {
			Dispose();
			PhotonNetwork.Disconnect();
			Application.Quit();
		}

		private void SetUser(string userName, string playfabId) {
			OnPlayfabLogin?.Invoke(playfabId);
			_userName = userName;
			_playfabId = playfabId;
			SetupScoresLable();
			_lobbyScreenController.Init(_userName);
			UpdateButtonsInteractivity();
		}

		private void SetupScoresLable() {
			if (string.IsNullOrEmpty(_playfabId)) {
				_mainMenuView.ScoreLable.gameObject.SetActive(false);
				return;
			}
			
			PlayFabClientAPI.GetUserData(new GetUserDataRequest {
					PlayFabId = _playfabId,
					Keys = new List<string> { TextConstants.WINS_AMOUNT_PLAYFAB_KEY, TextConstants.KILLS_AMOUNT_PLAYFAB_KEY }
			}, result => {
				if (result.Data != null) {
					var winsScores = 0;
					var killsScores = 0;
					if (result.Data.ContainsKey(TextConstants.WINS_AMOUNT_PLAYFAB_KEY) 
							&& int.TryParse(result.Data[TextConstants.WINS_AMOUNT_PLAYFAB_KEY].Value, out var winsAmount)) {
						_missionResultController.SetWinsAmount(winsAmount);
						winsScores = winsAmount * _mainMenuConfig.ScorePerWin;
					}
					if (result.Data.ContainsKey(TextConstants.KILLS_AMOUNT_PLAYFAB_KEY) 
							&& int.TryParse(result.Data[TextConstants.KILLS_AMOUNT_PLAYFAB_KEY].Value, out var killsAmount)) {
						_missionResultController.SetKillsAmount(killsAmount);
						killsScores = killsAmount * _mainMenuConfig.ScorePerKill;
					}
					_mainMenuView.ScoreLable.text = string.Format(_mainMenuConfig.ScoreLableTemplate, winsScores + killsScores);
					_mainMenuView.ScoreLable.gameObject.SetActive(winsScores > 0 || killsScores > 0);
				}
			}, errorCallback => Debug.LogWarning(errorCallback.ErrorMessage));		
		}

		private void UpdateButtonsInteractivity() {
			_mainMenuView.LoginButtonText.text = string.IsNullOrEmpty(_userName) ? TextConstants.LOGIN_TEXT : _userName;
			_mainMenuView.LoginPanelButton.interactable = string.IsNullOrEmpty(_userName);
			_mainMenuView.PlayButton.interactable = !string.IsNullOrEmpty(_userName);
		}

	}

}