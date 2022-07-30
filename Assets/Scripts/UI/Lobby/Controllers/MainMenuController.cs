using Infrastructure.Zenject;
using Photon.Pun;
using UnityEngine;
using Utils;
using Zenject;


namespace Infrastructure {

	internal class MainMenuController : IMainMenuController, IDisposer {
		
		private string _userName;
		private MainMenuView _mainMenuView;
		private LoginPanelController _loginPanelController;
		private LobbyScreenController _lobbyScreenController;
		private readonly LobbyConfig _lobbyConfig;
		private readonly IPermanentUiController _permanentUiController;


		[Inject]
		public MainMenuController(/*MainMenuView mainMenuView, */LobbyConfig lobbyConfig, IPermanentUiController permanentUiController) {
			// _mainMenuView = mainMenuView;
			_lobbyConfig = lobbyConfig;
			_permanentUiController = permanentUiController;
		}

		public void Dispose() {
			OnDispose();
		}

		public void OnDispose() {
			_mainMenuView.LoginPanelButton.onClick.RemoveAllListeners();
			_mainMenuView.PlayButton.onClick.RemoveAllListeners();
			_mainMenuView.QuitGameButton.onClick.RemoveAllListeners();
			_loginPanelController.OnUserLoginSuccess -= SetUser;
			_loginPanelController.Dispose();
			_lobbyScreenController.Dispose();
			Object.Destroy(_mainMenuView.GameObject);
		}

		public void SetupMainMenu() {
			if (_mainMenuView == null) {
				_mainMenuView = Object.Instantiate(_lobbyConfig.MainMenuPref);
			}
			_mainMenuView.GameObject.SetActive(true);
			_mainMenuView.HeaderLabel.text = TextConstants.MAIN_MENU_HEADER_TEXT;
			InitButtons();
			InitLoginPanel();
			InitLobbyPanel();
			UpdateButtonsInteractivity();


			void InitButtons() {
				_mainMenuView.LoginPanelButton.onClick.AddListener(OpenLoginPanel);
				_mainMenuView.PlayButton.onClick.AddListener(OpenPlayPanel);
				_mainMenuView.SettingsButton.onClick.AddListener(OpenSettingsPanel);
				_mainMenuView.CreditsButton.onClick.AddListener(ShowCredits);
				_mainMenuView.QuitGameButton.onClick.AddListener(QuitGame);
			}

			void InitLoginPanel() {
				_loginPanelController = new LoginPanelController(_mainMenuView.LoginPanelView);
				_loginPanelController.Init();
				_loginPanelController.OnUserLoginSuccess += SetUser;
			}

			void InitLobbyPanel() {
				_lobbyScreenController = new LobbyScreenController(_mainMenuView.LobbyScreenView, _lobbyConfig, _permanentUiController);
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
			
		}

		private void QuitGame() {
			OnDispose();
			PhotonNetwork.Disconnect();
			Application.Quit();
		}

		private void SetUser(string userName) {
			_userName = userName;
			_lobbyScreenController.Init(_userName);
			UpdateButtonsInteractivity();
		}

		private void UpdateButtonsInteractivity() {
			_mainMenuView.LoginButtonText.text = string.IsNullOrEmpty(_userName) ? TextConstants.LOGIN_TEXT : _userName;
			_mainMenuView.LoginPanelButton.interactable = string.IsNullOrEmpty(_userName);
			_mainMenuView.PlayButton.interactable = !string.IsNullOrEmpty(_userName);
		}

	}

}