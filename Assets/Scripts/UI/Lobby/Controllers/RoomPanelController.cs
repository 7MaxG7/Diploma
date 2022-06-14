using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using Utils;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class RoomPanelController : IDisposable {
		
		public event Action OnGameStarted;
		public RoomPanelView RoomPanelView { get; }

		private readonly LobbyConfig _lobbyConfig;
		private readonly ILobbyStatusDisplayer _lobbyScreenController;
		private Dictionary<Player, RoomPlayerItemView> _cachedPlayerItemViews = new();


		public RoomPanelController(LobbyConfig lobbyConfig, RoomPanelView roomPanelView, ILobbyStatusDisplayer lobbyScreenController) {
			_lobbyConfig = lobbyConfig;
			_lobbyScreenController = lobbyScreenController;
			RoomPanelView = roomPanelView;
		}

		public void Init() {
			RoomPanelView.StartGameButton.onClick.AddListener(StartGame);
			RoomPanelView.ClosePanelButton.onClick.AddListener(LeaveRoom);
		}

		public void Dispose() {
			OnDispose();
		}
		
		public void OnDispose() {
			RoomPanelView.StartGameButton.onClick.RemoveAllListeners();
			RoomPanelView.ClosePanelButton.onClick.RemoveAllListeners();
			DOTween.KillAll();
		}

		public Tween ShowPanel(string roomName) {
			ClearPanel();
			ToggleBlockingUi(true);
			RoomPanelView.gameObject.SetActive(true);
			RoomPanelView.RoomPanelHeader.text = roomName;
			RoomPanelView.CanvasGroup.alpha = 0;
			return RoomPanelView.CanvasGroup.DOFade(1, Constants.LOBBY_PANEL_FADING_DURATION);
		}

		public Tween HidePanel() {
			ToggleBlockingUi(true);
			return RoomPanelView.CanvasGroup.DOFade(0, Constants.LOBBY_PANEL_FADING_DURATION);
		}

		public void DeactivatePanel() {
			ClearPanel();
			RoomPanelView.gameObject.SetActive(false);
		}

		public void AddPlayer(Player player) {
			var playerItem = Object.Instantiate(_lobbyConfig.RoomCachedPlayerItemPref, RoomPanelView.PlayersListContent);
			playerItem.PlayerName.text = player.NickName;
			_cachedPlayerItemViews.Add(player, playerItem);
		}

		public void RemovePlayer(Player player) {
			if (_cachedPlayerItemViews.ContainsKey(player)) {
				Object.Destroy(_cachedPlayerItemViews[player].gameObject);
				_cachedPlayerItemViews.Remove(player);
			}
		}

		public void ToggleBlockingUi(bool mustBlocked) {
			RoomPanelView.StartGameButton.interactable = !mustBlocked;
			RoomPanelView.ClosePanelButton.interactable = !mustBlocked;
		}

		private void StartGame() {
			PhotonNetwork.CurrentRoom.IsOpen = false;
			PhotonNetwork.CurrentRoom.IsVisible = false;
			ClearPanel();
			
			PhotonNetwork.LoadLevel(TextConstants.MISSION_SCENE_NAME);
		}

		private void LeaveRoom() {
			ToggleBlockingUi(true);
			PhotonNetwork.LeaveRoom();
		}

		private void ClearPanel() {
			foreach (var playerItem in _cachedPlayerItemViews.Values) {
				Object.Destroy(playerItem.gameObject);
			}
			_cachedPlayerItemViews.Clear();
		}
	}

}