using UnityEngine;
using UnityEngine.UI;


namespace UI {

	internal class ResultPanelView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private Button _closePanelButton;

		public CanvasGroup CanvasGroup => _canvasGroup;
		public Button ClosePanelButton => _closePanelButton;
	}

}