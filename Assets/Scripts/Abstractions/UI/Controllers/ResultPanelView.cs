using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI {

	internal class ResultPanelView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private TMP_Text _resultLableText;
		[SerializeField] private Button _closePanelButton;
		[SerializeField] private TMP_Text _killsAmount;

		public CanvasGroup CanvasGroup => _canvasGroup;
		public TMP_Text ResultLableText => _resultLableText;
		public Button ClosePanelButton => _closePanelButton;
		public TMP_Text KillsAmount => _killsAmount;
	}

}