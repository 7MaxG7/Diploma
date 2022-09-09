using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Utils;


namespace Infrastructure {

	internal class RoomPanelController : IDisposable {
		private readonly RoomPanelView _roomPanelView;
		private readonly MainMenuConfig _mainMenuConfig;
		private readonly IPermanentUiController _permanentUiController;
		private readonly Dictionary<Player, RoomPlayerItemView> _cachedPlayerItemViews = new();
		private readonly List<Player> _players = new();


		public RoomPanelController(MainMenuConfig mainMenuConfig, RoomPanelView roomPanelView, IPermanentUiController permanentUiController) {
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
			
			// ClearPanel();
			// ToggleBlockingUi(true);
			// _roomPanelView.gameObject.SetActive(true);
			// _roomPanelView.RoomPanelHeader.text = roomName;
			// _roomPanelView.CanvasGroup.alpha = 0;
			// _roomPanelView.CanvasGroup.DOFade(1, Constants.LOBBY_PANEL_FADING_DURATION)
			// 		.OnComplete(() => {
			// 				onPanelShownCallback?.Invoke();
			// 				ToggleBlockingUi(false);
			// 		});
		}

		private void UpdateMasterButtons() {
			_roomPanelView.ToggleMasterButtons(PhotonNetwork.IsMasterClient);
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
			PhotonNetwork.CurrentRoom.IsOpen = false;
			PhotonNetwork.CurrentRoom.IsVisible = false;
			_roomPanelView.BlockUi();
			_permanentUiController.OnCurtainShown += LoadMission;
			_permanentUiController.ShowLoadingCurtain();
		}

		private void LoadMission() {
			_permanentUiController.OnCurtainShown -= LoadMission;
			PhotonNetwork.LoadLevel(TextConstants.MISSION_SCENE_NAME);
		}

		private void LeaveRoom() {
			_roomPanelView.BlockUi();
			PhotonNetwork.LeaveRoom();
		}
	}

}