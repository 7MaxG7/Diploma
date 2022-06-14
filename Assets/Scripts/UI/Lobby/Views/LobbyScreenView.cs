using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure {

	internal class LobbyScreenView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private TMP_Text _statusLableText;
		[SerializeField] private LobbyPanelView _lobbyPanelView;
		[SerializeField] private RoomPanelView _roomPanelView;

		public CanvasGroup CanvasGroup => _canvasGroup;
		public TMP_Text StatusLableText => _statusLableText;
		public LobbyPanelView LobbyPanelView => _lobbyPanelView;
		public RoomPanelView RoomPanelView => _roomPanelView;

	}

}