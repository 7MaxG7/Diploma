using System;
using Photon.Pun;
using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class MainMenuController : IMainMenuController, IDisposer {
		
		private string _userName;
		private readonly IMainMenuView _mainMenuView;
		private LoginPanelController _loginPanelController;
		private LobbyScreenController _lobbyScreenController;
		private readonly LobbyConfig _lobbyConfig;


		[Inject]
		public MainMenuController(IMainMenuView mainMenuView, LobbyConfig lobbyConfig) {
			_mainMenuView = mainMenuView;
			_lobbyConfig = lobbyConfig;
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
			_mainMenuView.GameObject.SetActive(true);
			_mainMenuView.HeaderLabel.text = TextConstants.MAIN_MENU_HEADER_TEXT;
			InitButtons();
			InitLoginPanel();
			InitLobbyPanel();
			UpdateButtonsInteractivity();


			void InitButtons() {
				_mainMenuView.LoginPanelButton.onClick.AddListener(OpenLoginPanel);
				_mainMenuView.PlayButton.onClick.AddListener(OpenPlayPanel);
				_mainMenuView.QuitGameButton.onClick.AddListener(QuitGame);
			}

			void InitLoginPanel() {
				_loginPanelController = new LoginPanelController(_mainMenuView.LoginPanelView);
				_loginPanelController.Init();
				_loginPanelController.OnUserLoginSuccess += SetUser;
			}

			void InitLobbyPanel() {
				_lobbyScreenController = new LobbyScreenController(_mainMenuView.LobbyScreenView, _lobbyConfig);
			}
		}

		private void OpenLoginPanel() {
			_loginPanelController.ShowPanel();
		}

		private void OpenPlayPanel() {
			_lobbyScreenController.ShowScreen();
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