using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Utils;


namespace Infrastructure {

	internal class LobbyScreenController : IConnectionCallbacks, ILobbyCallbacks, IMatchmakingCallbacks, IInRoomCallbacks, ILobbyStatusDisplayer
			, IDisposer, IDisposable {
		private const int LOADING_UPDATE_DELAY = 250;

		private string _userName;
		private readonly LobbyScreenView _lobbyScreenView;
		private readonly LobbyPanelController _lobbyPanelController;
		private readonly RoomPanelController _roomPanelController;
		private bool _isFading;
		private bool _uiIsBlocked;

		public bool IsLoading { set; get; }


		public LobbyScreenController(LobbyScreenView lobbyScreenView, MainMenuConfig mainMenuConfig, IPermanentUiController permanentUiController) {
			_lobbyScreenView = lobbyScreenView;
			_lobbyPanelController = new LobbyPanelController(mainMenuConfig, _lobbyScreenView.LobbyPanelView, this);
			_roomPanelController = new RoomPanelController(mainMenuConfig, lobbyScreenView.RoomPanelView, permanentUiController);
		}

		public void Dispose() {
			OnDispose();
			PhotonNetwork.RemoveCallbackTarget(this);
			DOTween.Clear();
		}

		public void Init(string userName) {
			_userName = userName;
			InitPhoton();
			_lobbyPanelController.Init();
			_roomPanelController.Init();


			void InitPhoton() {
				PhotonNetwork.LocalPlayer.NickName = _userName;
				PhotonNetwork.AddCallbackTarget(this);
				PhotonNetwork.AutomaticallySyncScene = true;
			}
		}

		public void OnDispose() {
			_lobbyPanelController.Dispose();
			_roomPanelController.Dispose();
			DOTween.KillAll();
		}
		
#region IConnectionCallbacks
		public void OnConnectedToMaster() {
			PhotonNetwork.JoinLobby(new TypedLobby("customLobby", LobbyType.Default));
		}

		public void OnDisconnected(DisconnectCause cause) {
			HideScreen();
		}

		public void OnConnected() { }

		public void OnRegionListReceived(RegionHandler regionHandler) { }

		public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

		public void OnCustomAuthenticationFailed(string debugMessage) { }
#endregion
		
#region ILobbyCallbacks
		public void OnJoinedLobby() {
			_lobbyPanelController.ToggleBlockingUi(false);
			IsLoading = false;
		}

		public void OnLeftLobby() {
			PhotonNetwork.Disconnect();
		}

		public void OnRoomListUpdate(List<RoomInfo> roomList) {
			_lobbyPanelController.UpdateCachedRoomList(roomList);
		}

		public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) { }
#endregion
		
#region IMatchmakingCallbacks
		public void OnJoinedRoom() {
			_lobbyPanelController.HidePanel()
					.OnComplete(() => {
						_roomPanelController.ShowPanel(PhotonNetwork.CurrentRoom.Name)
								.OnComplete(() => {
										_lobbyPanelController.DeactivatePanel();
										IsLoading = false;
										_roomPanelController.ToggleBlockingUi(false);
								});
						foreach (var player in PhotonNetwork.CurrentRoom.Players.Values) {
							_roomPanelController.AddPlayer(player);
						}
					});
		}

		public void OnLeftRoom() {
			_roomPanelController.HidePanel()
					.OnComplete(() => {
							_lobbyPanelController.ShowPanel()
									.OnComplete(() => {
											_roomPanelController.DeactivatePanel();
											IsLoading = false;
											_lobbyPanelController.ToggleBlockingUi(false);
									});
					});
		}

		public void OnCreateRoomFailed(short returnCode, string message) {
			_lobbyPanelController.ToggleBlockingUi(false);
			IsLoading = false;
		}

		public void OnJoinRoomFailed(short returnCode, string message) {
			_lobbyPanelController.ToggleBlockingUi(false);
			IsLoading = false;
		}

		public void OnJoinRandomFailed(short returnCode, string message) {
			_lobbyPanelController.ToggleBlockingUi(false);
			IsLoading = false;
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
		
		public void ShowScreen() {
			PhotonNetwork.ConnectUsingSettings();
			if (_isFading) {
				_lobbyScreenView.CanvasGroup.DOKill();
			}
			ShowLobbyScreenWithAnimation();
			_roomPanelController.RoomPanelView.gameObject.SetActive(false);
			_lobbyPanelController.ShowPanel();
			IsLoading = true;
			ShowLoadingStatusAsync();


			void ShowLobbyScreenWithAnimation() {
				_isFading = true;
				_lobbyScreenView.gameObject.SetActive(true);
				_lobbyScreenView.CanvasGroup.alpha = 0;
				_lobbyScreenView.CanvasGroup.DOFade(1, Constants.LOBBY_PANEL_FADING_DURATION)
						.OnComplete(() => { _isFading = false; });
			}
		}

		private void HideScreen() {
			if (_isFading) {
				_lobbyScreenView.CanvasGroup.DOKill();
			}
			_isFading = true;
			_lobbyScreenView.CanvasGroup.DOFade(0, Constants.LOGIN_PANEL_FADING_DURATION)
					.OnComplete(
							() => {
								_lobbyPanelController.ClearPanel();
								_lobbyScreenView.gameObject.SetActive(false);
								_isFading = false;
								IsLoading = false;
							}
					);
		}

		public async void ShowLoadingStatusAsync() {
			const int startAwaitingTick = 0;
			const int maxAwaitingTick = 5;
			_lobbyScreenView.StatusLableText.color = Color.cyan;
			var currentAwaitingTick = startAwaitingTick;
			while (IsLoading) {
				_lobbyScreenView.StatusLableText.text = $"{TextConstants.LOADING_STATUS_TEXT_TEMPLATE}{new string('.', currentAwaitingTick)}";
				if (++currentAwaitingTick >= maxAwaitingTick)
					currentAwaitingTick = startAwaitingTick;
				await Task.Delay(LOADING_UPDATE_DELAY);
			}
			_lobbyScreenView.StatusLableText.text = string.Empty;
		}
	}

}