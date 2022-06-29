using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure {

	internal class LobbyPanelView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private Transform _roomsListContent;
		[SerializeField] private TMP_InputField _privateRoomNameInputText;
		[SerializeField] private Button _createPrivateRoomButton;
		[SerializeField] private Button _joinPrivateRoomButton;
		[SerializeField] private Button _createNewRoomButton;
		[SerializeField] private Button _joinRandomRoomButton;
		[SerializeField] private Button _closePanelButton;

		public CanvasGroup CanvasGroup => _canvasGroup;
		public Transform RoomsListContent => _roomsListContent;
		public TMP_InputField PrivateRoomNameInputText => _privateRoomNameInputText;
		public Button CreatePrivateRoomButton => _createPrivateRoomButton;
		public Button JoinPrivateRoomButton => _joinPrivateRoomButton;
		public Button CreateNewRoomButton => _createNewRoomButton;
		public Button JoinRandomRoomButton => _joinRandomRoomButton;
		public Button ClosePanelButton => _closePanelButton;
	}

}