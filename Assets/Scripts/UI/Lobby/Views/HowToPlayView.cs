using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure {

	internal class HowToPlayView : MonoBehaviour {
		[SerializeField] private GameObject _gameObject;
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private HorizontalLayoutGroup _contentHorizontalGroup;
		[SerializeField] private ScrollRect _rulesScroll;
		[SerializeField] private Button _prevPageButton;
		[SerializeField] private Button _nextPageButton;
		[SerializeField] private Button _hideHowToPlayButton;

		public GameObject GameObject => _gameObject;
		public CanvasGroup CanvasGroup => _canvasGroup;
		public ScrollRect RulesScroll => _rulesScroll;
		public Button PrevPageButton => _prevPageButton;
		public Button NextPageButton => _nextPageButton;
		public Button HideHowToPlayButton => _hideHowToPlayButton;
		public HorizontalLayoutGroup ContentHorizontalGroup => _contentHorizontalGroup;
	}

}