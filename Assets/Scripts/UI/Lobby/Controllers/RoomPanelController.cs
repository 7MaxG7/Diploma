using System;
using System.Collections.Generic;
using Photon.Realtime;
using Services;
using Utils;


namespace Infrastructure {

	internal sealed class RoomPanelController : IDisposable {
		private readonly RoomPanelView _roomPanelView;
		private readonly IPhotonManager _photonManager;
		private readonly MainMenuConfig _mainMenuConfig;
		private readonly IPermanentUiController _permanentUiController;
		private readonly List<Player> _players = new();


		public RoomPanelController(IPhotonManager photonManager, MainMenuConfig mainMenuConfig, RoomPanelView roomPanelView
				, IPermanentUiController permanentUiController) {
			_photonManager = photonManager;
			_mainMenuConfig = mainMenuConfig;
			_permanentUiController = permanentUiController;
			_roomPanelView = roomPanelView;
		}

		public void Dispose() {
			_roomPanelView.OnStartGameClick -= StartGame;
			_roomPanelView.OnClosePanelClick -= LeaveRoom;
			_roomPanelView.OnDispose();
		}

		public void Init() {
			_roomPanelView.Init(_mainMenuConfig);
			_roomPanelView.OnStartGameClick += StartGame;
			_roomPanelView.OnClosePanelClick += LeaveRoom;
		}

		public void ShowPanel(string roomName, Action onPanelShownCallback = null) {
			_roomPanelView.Show(roomName, onPanelShownCallback);
			UpdateMasterButtons();
		}

		private void UpdateMasterButtons() {
			_roomPanelView.ToggleMasterButtons(_photonManager.IsMasterClient);
		}

		public void HidePanel(Action onPanelHiddenCallback) {
			_roomPanelView.Hide(onPanelHiddenCallback: onPanelHiddenCallback);
		}

		public void DeactivatePanel() {
			_roomPanelView.Hide(false);
		}

		public void AddPlayer(Player player) {
			if (!_players.Contains(player)) {
				_roomPanelView.AddPlayerItem(player.NickName);
				_players.Add(player);
			}
		}

		public void RemovePlayer(Player player) {
			if (_players.Remove(player)) {
				_roomPanelView.RemovePlayerItem(player.NickName);
			}
			UpdateMasterButtons();
		}

		private void StartGame() {
			_photonManager.SetRoomParameters(isOpened: false, isVisible: false);
			_roomPanelView.BlockUi();
			_permanentUiController.OnCurtainShown += LoadMission;
			_permanentUiController.ShowLoadingCurtain();
		}

		private void LoadMission() {
			_permanentUiController.OnCurtainShown -= LoadMission;
			_photonManager.LoadLevel(Constants.MISSION_SCENE_NAME);
		}

		private void LeaveRoom() {
			_roomPanelView.BlockUi();
			_photonManager.LeaveRoom();
		}
	}

}