using System;
using System.Collections.Generic;
using Abstractions.Services;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Utils;
using Zenject;


namespace Infrastructure {

	internal class MainMenuController : IMainMenuController {
		public event Action<string> OnPlayfabLogin;
		
		private string _userName;
		private string _playfabId;
		private MainMenuView _mainMenuView;
		private LoginPanelController _loginPanelController;
		private LobbyScreenController _lobbyScreenController;
		private readonly IMissionResultController _missionResultController;
		private readonly IPermanentUiController _permanentUiController;
		private readonly IViewsFactory _viewsFactory;
		private readonly MainMenuConfig _mainMenuConfig;
		private bool _rulesPageIsMoving;


		[Inject]
		public MainMenuController(IMissionResultController missionResultController, IPermanentUiController permanentUiController
				, IViewsFactory viewsFactory, MainMenuConfig mainMenuConfig) {
			_missionResultController = missionResultController;
			_viewsFactory = viewsFactory;
			_permanentUiController = permanentUiController;
			_mainMenuConfig = mainMenuConfig;
		}

		public void Dispose() {
			_mainMenuView.OnLoginClick -= _loginPanelController.ShowPanel;
			_mainMenuView.OnPlayClick -= _lobbyScreenController.ShowScreen;
			_mainMenuView.OnSettingsClick -= OpenSettingsPanel;
			_mainMenuView.OnHowToPlayClick -= _mainMenuView.HowToPlayView.ShowRules;
			_mainMenuView.HowToPlayView.OnPrevPageClick -= _mainMenuView.HowToPlayView.ShowPrevPage;
			_mainMenuView.HowToPlayView.OnNextPageClick -= _mainMenuView.HowToPlayView.ShowNextPage;
			_mainMenuView.HowToPlayView.OnHidePanelClick -= _mainMenuView.HowToPlayView.HideRules;
			_mainMenuView.OnCreditsClick -= _mainMenuView.CreditsView.ShowCredits;
			_mainMenuView.CreditsView.OnCloseCreditsClick -= _mainMenuView.CreditsView.HideCredits;
			_mainMenuView.OnQuitGameClick -= QuitGame;
			_loginPanelController.OnUserLoginSuccess -= SetUser;
			_loginPanelController.Dispose();
			_lobbyScreenController.Dispose();
			_mainMenuView.HowToPlayView.Dispose();
			_mainMenuView.CreditsView.Dispose();
			_mainMenuView.Dispose();
		}


		public void SetupMainMenu() {
			if (_mainMenuView == null) {
				_mainMenuView = _viewsFactory.CreateMainMenu();
			}
			_mainMenuView.Init(_mainMenuConfig);
			InitHowToPlay();
			InitCredits();
			InitLoginPanel();
			InitLobbyPanel();
			InitButtons();
			SetupScoresLable();

			void InitHowToPlay() {
				_mainMenuView.HowToPlayView.Init(_mainMenuConfig);
				_mainMenuView.HowToPlayView.OnPrevPageClick += _mainMenuView.HowToPlayView.ShowPrevPage;
				_mainMenuView.HowToPlayView.OnNextPageClick += _mainMenuView.HowToPlayView.ShowNextPage;
				_mainMenuView.HowToPlayView.OnHidePanelClick += _mainMenuView.HowToPlayView.HideRules;
			}

			void InitCredits() {
				_mainMenuView.CreditsView.Init(_mainMenuConfig);
				_mainMenuView.CreditsView.OnCloseCreditsClick += _mainMenuView.CreditsView.HideCredits;
			}

			void InitLoginPanel() {
				_loginPanelController = new LoginPanelController(_mainMenuView.LoginPanelView, _mainMenuConfig);
				_loginPanelController.Init();
				_loginPanelController.OnUserLoginSuccess += SetUser;
			}

			void InitLobbyPanel() {
				_lobbyScreenController = new LobbyScreenController(_mainMenuView.LobbyScreenView, _mainMenuConfig, _permanentUiController);
				if (PlayFabClientAPI.IsClientLoggedIn()) {
					_lobbyScreenController.Init(_userName);
				}
			}

			void InitButtons() {
				_mainMenuView.OnLoginClick += _loginPanelController.ShowPanel;
				_mainMenuView.OnPlayClick += _lobbyScreenController.ShowScreen;
				_mainMenuView.OnSettingsClick += OpenSettingsPanel;
				_mainMenuView.OnHowToPlayClick += _mainMenuView.HowToPlayView.ShowRules;
				_mainMenuView.OnCreditsClick += _mainMenuView.CreditsView.ShowCredits;
				_mainMenuView.OnQuitGameClick += QuitGame;
				_mainMenuView.UpdateLoginButtons(_userName);
			}
		}

		private void OpenSettingsPanel() {
			_permanentUiController.ShowSettingsPanel();
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
			_mainMenuView.UpdateLoginButtons(_userName);
		}

		private void SetupScoresLable() {
			if (string.IsNullOrEmpty(_playfabId)) {
				_mainMenuView.SetScore(null);
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
					_mainMenuView.SetScore(winsScores + killsScores);
				}
			}, errorCallback => Debug.LogWarning(errorCallback.ErrorMessage));
		}
	}

}