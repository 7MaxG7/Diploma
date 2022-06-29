using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure {

	internal class RoomPanelView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private Transform _playersListContent;
		[SerializeField] private TMP_Text _roomPanelHeader;
		[SerializeField] private Button _startGameButton;
		[SerializeField] private Button _closePanelButton;

		public CanvasGroup CanvasGroup => _canvasGroup;
		public TMP_Text RoomPanelHeader => _roomPanelHeader;
		public Transform PlayersListContent => _playersListContent;
		public Button StartGameButton => _startGameButton;
		public Button ClosePanelButton => _closePanelButton;
	}

}