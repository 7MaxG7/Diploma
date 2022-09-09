using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.UI.Controllers;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;


namespace Infrastructure {

	internal class LobbyPanelController : IDisposable {
		private readonly LobbyPanelView _lobbyPanelView;
		private readonly MainMenuConfig _mainMenuConfig;
		private readonly ILobbyStatusDisplayer _lobbyStatusDisplayer;
		private readonly List<RoomInfo> _roomsList = new();
		private bool _uiIsBlocked;
		private IRoomEventsCallbacks _lobbyScreenController;


		public LobbyPanelController(MainMenuConfig mainMenuConfig, LobbyPanelView lobbyPanelView, ILobbyStatusDisplayer lobbyStatusDisplayer) {
			_lobbyPanelView = lobbyPanelView;
			_mainMenuConfig = mainMenuConfig;
			_lobbyStatusDisplayer = lobbyStatusDisplayer;
		}

		public void Dispose() {
			DOTween.KillAll();
			_lobbyPanelView.Dispose();
			_lobbyPanelView.OnCreatePrivateRoomClick -= CreatePrivateRoom;
			_lobbyPanelView.OnJoinPrivateRoomClick -= JoinRoom;
			_lobbyPanelView.OnCreateNewRoomClick -= CreateRoom;
			_lobbyPanelView.OnJoinRandomRoomClick -= JoinOrCreateRandomRoom;
			_lobbyPanelView.OnClosePanelClick -= Disconnect;
			_lobbyPanelView.OnJoinRoomClick -= JoinRoom;
			_lobbyScreenController.OnLobbyJoin -= _lobbyPanelView.UnblockUi;
			_lobbyScreenController.OnRoomCreationFail -= _lobbyPanelView.UnblockUi;
			_lobbyScreenController.OnRoomJoinFail -= _lobbyPanelView.UnblockUi;
			_lobbyScreenController.OnRandomRoomJoinFail -= _lobbyPanelView.UnblockUi;
		}

		public void Init(IRoomEventsCallbacks lobbyScreenController) {
			_lobbyScreenController = lobbyScreenController;
			_lobbyPanelView.Init(_mainMenuConfig);
			_lobbyPanelView.OnCreatePrivateRoomClick += CreatePrivateRoom;
			_lobbyPanelView.OnJoinPrivateRoomClick += JoinRoom;
			_lobbyPanelView.OnCreateNewRoomClick += CreateRoom;
			_lobbyPanelView.OnJoinRandomRoomClick += JoinOrCreateRandomRoom;
			_lobbyPanelView.OnClosePanelClick += Disconnect;
			_lobbyPanelView.OnJoinRoomClick += JoinRoom;
			_lobbyScreenController.OnLobbyJoin += _lobbyPanelView.UnblockUi;
			_lobbyScreenController.OnRoomCreationFail += _lobbyPanelView.UnblockUi;
			_lobbyScreenController.OnRoomJoinFail += _lobbyPanelView.UnblockUi;
			_lobbyScreenController.OnRandomRoomJoinFail += _lobbyPanelView.UnblockUi;
		}

		public void ShowPanel(Action onPanelShownCallback = null) {
			_lobbyPanelView.Show(onPanelShownCallback);
		}

		public void HidePanel(Action onPanelHiddenCallback = null) {
			_lobbyPanelView.Hide(onPanelHiddenCallback: onPanelHiddenCallback);
		}

		public void DeactivatePanel() {
			_lobbyPanelView.Hide(false);
		}

		public void UpdateRoomsList(List<RoomInfo> roomList) {
			foreach (var roomInfo in roomList) {
				if (roomInfo.RemovedFromList) {
					if (_roomsList.Remove(roomInfo)) {
						_lobbyPanelView.RemoveRoom(roomInfo);
					}
				} else {
					if (!_roomsList.Contains(roomInfo)) {
						_lobbyPanelView.AddRoom(roomInfo);
						_roomsList.Add(roomInfo);
					}
				}
			}
		}

		private void CreatePrivateRoom(string roomName) {
			_lobbyStatusDisplayer.ShowLoadingStatusAsync();
			var roomOptions = new RoomOptions { IsVisible = false };
			PhotonNetwork.CreateRoom(roomName, roomOptions);
		}

		private void CreateRoom() {
			var roomName = string.Format(_mainMenuConfig.RoomNameTemplate, PhotonNetwork.LocalPlayer.NickName, (int)PhotonNetwork.Time);
			var currentRoomsNames = _roomsList.Select(roomInfo => roomInfo.Name).ToArray();
			if (currentRoomsNames.Contains(roomName)) {
				var additionalIndex = 0;
				while (currentRoomsNames.Contains($"{roomName} {additionalIndex}")) {
					++additionalIndex;
				}
				roomName += $" {additionalIndex}";
			}
			_lobbyStatusDisplayer.ShowLoadingStatusAsync();
			PhotonNetwork.CreateRoom(roomName, new RoomOptions());
		}

		private void JoinRoom(string roomName) {
			_lobbyStatusDisplayer.ShowLoadingStatusAsync();
			PhotonNetwork.JoinRoom(roomName);
		}

		private void JoinOrCreateRandomRoom() {
			_lobbyStatusDisplayer.ShowLoadingStatusAsync();
			PhotonNetwork.JoinRandomOrCreateRoom();
		}

		private void Disconnect() {
			_lobbyStatusDisplayer.ShowLoadingStatusAsync();
			PhotonNetwork.LeaveLobby();
		}
	}

}
