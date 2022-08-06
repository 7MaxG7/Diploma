using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;


namespace Infrastructure {

	internal class CreditsView : MonoBehaviour {
		[SerializeField] private GameObject _gameObject;
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private ScrollRect _creditsScroll;
		[SerializeField] private Button _closeCreditsButton;

		public GameObject GameObject => _gameObject;
		public CanvasGroup CanvasGroup => _canvasGroup;
		public ScrollRect CreditsScroll => _creditsScroll;
		public Button CloseCreditsButton => _closeCreditsButton;
	}

}