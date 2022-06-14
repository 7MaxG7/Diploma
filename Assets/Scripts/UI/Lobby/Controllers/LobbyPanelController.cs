using System;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using Utils;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class LobbyPanelController : IDisposable {
		
		public LobbyPanelView LobbyPanelView { get; }

		private readonly LobbyConfig _lobbyConfig;
		private readonly ILobbyStatusDisplayer _lobbyStatusDisplayer;
		private readonly Dictionary<string, LobbyCachedRoomItemView> _cachedRoomItemViews = new();
		private bool _uiIsBlocked;


		public LobbyPanelController(LobbyConfig lobbyConfig, LobbyPanelView lobbyPanelView, ILobbyStatusDisplayer lobbyStatusDisplayer) {
			LobbyPanelView = lobbyPanelView;
			_lobbyConfig = lobbyConfig;
			_lobbyStatusDisplayer = lobbyStatusDisplayer;
		}

		public void Init() {
			LobbyPanelView.CreatePrivateRoomButton.onClick.AddListener(CreatePrivateRoom);
			LobbyPanelView.JoinPrivateRoomButton.onClick.AddListener(JoinPrivateRoom);
			LobbyPanelView.CreateNewRoomButton.onClick.AddListener(CreateRoom);
			LobbyPanelView.JoinRandomRoomButton.onClick.AddListener(JoinOrCreateRandomRoom);
			LobbyPanelView.ClosePanelButton.onClick.AddListener(Disconnect);
			LobbyPanelView.PrivateRoomNameInputText.onValueChanged.AddListener(UpdatePrivateRoomButtonsInteractivity);
		}

		public void Dispose() {
			OnDispose();
		}

		public void OnDispose() {
			LobbyPanelView.CreatePrivateRoomButton.onClick.RemoveAllListeners();
			LobbyPanelView.JoinPrivateRoomButton.onClick.RemoveAllListeners();
			LobbyPanelView.CreateNewRoomButton.onClick.RemoveAllListeners();
			LobbyPanelView.JoinRandomRoomButton.onClick.RemoveAllListeners();
			LobbyPanelView.ClosePanelButton.onClick.RemoveAllListeners();
			LobbyPanelView.PrivateRoomNameInputText.onValueChanged.RemoveAllListeners();
			DOTween.KillAll();
		}

		public Tween ShowPanel() {
			ClearPanel();
			ToggleBlockingUi(true);
			LobbyPanelView.gameObject.SetActive(true);
			LobbyPanelView.CanvasGroup.alpha = 0;
			return LobbyPanelView.CanvasGroup.DOFade(1, Constants.LOBBY_PANEL_FADING_DURATION);
		}

		public Tween HidePanel() {
			ToggleBlockingUi(true);
			return LobbyPanelView.CanvasGroup.DOFade(0, Constants.LOBBY_PANEL_FADING_DURATION);
		}

		public void DeactivatePanel() {
			ClearPanel();
			LobbyPanelView.gameObject.SetActive(false);
		}

		public void ToggleBlockingUi(bool mustBlocked) {
			_uiIsBlocked = mustBlocked;
			if (mustBlocked) {
				LobbyPanelView.CreatePrivateRoomButton.interactable = false;
				LobbyPanelView.JoinPrivateRoomButton.interactable = false;
			} else {
				UpdatePrivateRoomButtonsInteractivity(LobbyPanelView.PrivateRoomNameInputText.text);
			}
			LobbyPanelView.CreateNewRoomButton.interactable = !mustBlocked;
			LobbyPanelView.JoinRandomRoomButton.interactable = !mustBlocked;
			LobbyPanelView.ClosePanelButton.interactable = !mustBlocked;

			foreach (var roomItem in _cachedRoomItemViews.Values) {
				roomItem.RoomButton.interactable = !mustBlocked;
			}
		}

		public void UpdateCachedRoomList(List<RoomInfo> roomList) {
			foreach (var roomInfo in roomList) {
				var roomName = roomInfo.Name;
				if (roomInfo.RemovedFromList) {
					if (_cachedRoomItemViews.ContainsKey(roomName)) {
						Object.Destroy(_cachedRoomItemViews[roomName].gameObject);
						_cachedRoomItemViews.Remove(roomName);
					}
				} else {
					if (!_cachedRoomItemViews.ContainsKey(roomName)) {
						var roomItem = Object.Instantiate(_lobbyConfig.LobbyCachedRoomItemPref, LobbyPanelView.RoomsListContent);
						roomItem.RoomName.text = roomName;
						roomItem.RoomButton.onClick.AddListener(() => this.JoinRoom(roomName));
						_cachedRoomItemViews[roomName] = roomItem;
					}
					
					var roomNameColor = _cachedRoomItemViews[roomName].RoomName.color;
					roomNameColor.a = roomInfo.IsOpen ? Constants.UNLOCKED_ROOM_LABEL_TRANSPARENCY : Constants.LOCKED_ROOM_LABEL_TRANSPARENCY;
					_cachedRoomItemViews[roomName].RoomName.color = roomNameColor;
				}
			}
		}

		public void ClearPanel() {
			foreach (var roomItemView in _cachedRoomItemViews.Values) {
				Object.Destroy(roomItemView.gameObject);
			}
			_cachedRoomItemViews.Clear();
		}

		private void UpdatePrivateRoomButtonsInteractivity(string privateRoomName) {
			if (_uiIsBlocked)
				return;

			LobbyPanelView.CreatePrivateRoomButton.interactable = !string.IsNullOrEmpty(privateRoomName) && !_cachedRoomItemViews.ContainsKey(privateRoomName);
			LobbyPanelView.JoinPrivateRoomButton.interactable = !string.IsNullOrEmpty(privateRoomName);
		}

#region LocalMethods
		private void CreatePrivateRoom() {
			var roomName = LobbyPanelView.PrivateRoomNameInputText.text;
			if (_cachedRoomItemViews.ContainsKey(roomName))
				return;
			
			_lobbyStatusDisplayer.IsLoading = true;
			_lobbyStatusDisplayer.ShowLoadingStatusAsync();
			var roomOptions = new RoomOptions { IsVisible = false };
			PhotonNetwork.CreateRoom(LobbyPanelView.PrivateRoomNameInputText.text, roomOptions);
		}

		private void CreateRoom() {
			var roomName = string.Format(TextConstants.ROOM_NAME_TEMPLATE, PhotonNetwork.LocalPlayer.NickName, (int)PhotonNetwork.Time);
			if (_cachedRoomItemViews.ContainsKey(roomName))
				return;

			_lobbyStatusDisplayer.IsLoading = true;
			_lobbyStatusDisplayer.ShowLoadingStatusAsync();
			PhotonNetwork.CreateRoom(roomName, new RoomOptions());
		}

		private void JoinPrivateRoom() {
			JoinRoom(LobbyPanelView.PrivateRoomNameInputText.text);
		}

		private void JoinRoom(string roomName) {
			_lobbyStatusDisplayer.IsLoading = true;
			_lobbyStatusDisplayer.ShowLoadingStatusAsync();
			PhotonNetwork.JoinRoom(roomName);
		}

		private void JoinOrCreateRandomRoom() {
			_lobbyStatusDisplayer.IsLoading = true;
			_lobbyStatusDisplayer.ShowLoadingStatusAsync();
			PhotonNetwork.JoinRandomOrCreateRoom();
		}

		private void Disconnect() {
			ToggleBlockingUi(true);
			_lobbyStatusDisplayer.IsLoading = true;
			_lobbyStatusDisplayer.ShowLoadingStatusAsync();
			PhotonNetwork.LeaveLobby();
		}
#endregion
	}

}
