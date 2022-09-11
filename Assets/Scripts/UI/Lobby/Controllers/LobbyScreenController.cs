using System;
using System.Collections.Generic;
using Abstractions.UI.Controllers;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Services;


namespace Infrastructure {

	internal sealed class LobbyScreenController : IConnectionCallbacks, ILobbyCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
			, ILobbyStatusDisplayer, IRoomEventsCallbacks, IDisposable {
		public event Action OnLobbyJoin;
		public event Action OnRoomCreationFail;
		public event Action OnRoomJoinFail;
		public event Action OnRandomRoomJoinFail;
		
		private readonly IPhotonManager _photonManager;
		private readonly LobbyScreenView _lobbyScreenView;
		private readonly MainMenuConfig _mainMenuConfig;
		private readonly LobbyPanelController _lobbyPanelController;
		private readonly RoomPanelController _roomPanelController;
		private string _userName;
		private bool _uiIsBlocked;


		public LobbyScreenController(IPhotonManager photonManager, LobbyScreenView lobbyScreenView, MainMenuConfig mainMenuConfig
				, IPermanentUiController permanentUiController) {
			_photonManager = photonManager;
			_lobbyScreenView = lobbyScreenView;
			_mainMenuConfig = mainMenuConfig;
			_lobbyPanelController = new LobbyPanelController(_photonManager, mainMenuConfig, _lobbyScreenView.LobbyPanelView, this);
			_roomPanelController = new RoomPanelController(_photonManager, mainMenuConfig, _lobbyScreenView.RoomPanelView, permanentUiController);
		}

		public void Dispose() {
			_lobbyPanelController.Dispose();
			_roomPanelController.Dispose();
			DOTween.Clear();
			DOTween.KillAll();
			PhotonNetwork.RemoveCallbackTarget(this);
		}
		
#region IConnectionCallbacks
		public void OnConnectedToMaster() {
			_photonManager.JoinCustonLobby();
		}

		public void OnDisconnected(DisconnectCause cause) {
			_lobbyScreenView.HideScreen(screenHiddenCallback: _lobbyPanelController.DeactivatePanel);
		}

		public void OnConnected() { }

		public void OnRegionListReceived(RegionHandler regionHandler) { }

		public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

		public void OnCustomAuthenticationFailed(string debugMessage) { }
#endregion
		
#region ILobbyCallbacks
		public void OnJoinedLobby() {
			_lobbyScreenView.StopLoadingStatus();
			OnLobbyJoin?.Invoke();
		}

		public void OnLeftLobby() {
			_photonManager.Disconnect();
		}

		public void OnRoomListUpdate(List<RoomInfo> roomList) {
			_lobbyPanelController.UpdateRoomsList(roomList);
		}

		public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) { }
#endregion
		
#region IMatchmakingCallbacks
		public void OnJoinedRoom() {
			SwitchLobbyToRoomPanel();
		}

		public void OnLeftRoom() {
			SwitchRoomToLobbyPanel();
		}

		public void OnCreateRoomFailed(short returnCode, string message) {
			_lobbyScreenView.StopLoadingStatus();
			OnRoomCreationFail?.Invoke();
		}

		public void OnJoinRoomFailed(short returnCode, string message) {
			_lobbyScreenView.StopLoadingStatus();
			OnRoomJoinFail?.Invoke();
		}

		public void OnJoinRandomFailed(short returnCode, string message) {
			_lobbyScreenView.StopLoadingStatus();
			OnRandomRoomJoinFail?.Invoke();
		}

		public void OnFriendListUpdate(List<FriendInfo> friendList) { }

		public void OnCreatedRoom() { }
#endregion

#region IInRoomCallbacks
		public void OnPlayerEnteredRoom(Player newPlayer) {
			_roomPanelController.AddPlayer(newPlayer);
		}

		public void OnPlayerLeftRoom(Player otherPlayer) {
			_roomPanelController.RemovePlayer(otherPlayer);
		}

		public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) { }

		public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) { }

		public void OnMasterClientSwitched(Player newMasterClient) { }
#endregion


		public void Init(string userName) {
			_userName = userName;
			InitSubcontrollers();
			InitPhoton();

			void InitSubcontrollers() {
				_lobbyScreenView.Init(_mainMenuConfig);
				_lobbyPanelController.Init(this);
				_roomPanelController.Init();
			}

			void InitPhoton() {
				_photonManager.PlayerName = _userName;
				PhotonNetwork.AutomaticallySyncScene = true;
				PhotonNetwork.AddCallbackTarget(this);
			}
		}

		public void ShowScreen() {
			_lobbyScreenView.Show();
			_roomPanelController.DeactivatePanel();
			_lobbyPanelController.ShowPanel();
			_photonManager.Connect();
		}

		public void ShowLoadingStatusAsync() {
			_lobbyScreenView.ShowLoadingStatusAsync();
		}
	
		private void SwitchLobbyToRoomPanel() {
			_lobbyPanelController.HidePanel(onPanelHiddenCallback: ShowRoomPanel);

			void ShowRoomPanel() {
				_roomPanelController.ShowPanel(_photonManager.RoomName, onPanelShownCallback: FinishRoomPanelShown);
				foreach (var player in _photonManager.GetRoomPlayers().Values) {
					_roomPanelController.AddPlayer(player);
				}
			}

			void FinishRoomPanelShown() {
				_lobbyPanelController.DeactivatePanel();
				_lobbyScreenView.StopLoadingStatus();
			}
		}
	
		private void SwitchRoomToLobbyPanel() {
			_roomPanelController.HidePanel(
					onPanelHiddenCallback: () => _lobbyPanelController.ShowPanel(onPanelShownCallback: FinishLobbyPanelShown)
			);

			void FinishLobbyPanelShown() {
				_roomPanelController.DeactivatePanel();
				_lobbyScreenView.StopLoadingStatus();
			}
		}
	}

}