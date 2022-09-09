using System;
using System.Collections;
using DG.Tweening;
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

		public event Action OnPrevPageClick;
		public event Action OnNextPageClick;
		public event Action OnHidePanelClick;
		
		private MainMenuConfig _mainMenuConfig;
		private bool _pageIsMoving;


		public void Init(MainMenuConfig mainMenuConfig) {
			_mainMenuConfig = mainMenuConfig;
			_prevPageButton.onClick.AddListener(() => OnPrevPageClick?.Invoke());
			_nextPageButton.onClick.AddListener(() => OnNextPageClick?.Invoke());
			_hideHowToPlayButton.onClick.AddListener(() => OnHidePanelClick?.Invoke());
		}

		public void ShowRules() {
			_canvasGroup.DOKill();
			gameObject.SetActive(true);
			_canvasGroup.alpha = 0;
			_rulesScroll.content.anchoredPosition = Vector2.zero;
			UpdateButtonsInteractivity();
			_canvasGroup.DOFade(1, _mainMenuConfig.RulesFadingDuration);
		}

		public void HideRules() {
			_canvasGroup.DOKill();
			StopAllCoroutines();
			_canvasGroup.DOFade(0, _mainMenuConfig.RulesFadingDuration)
					.OnComplete(() => gameObject.SetActive(false));
		}

		private void UpdateButtonsInteractivity() {
			_nextPageButton.interactable = !_pageIsMoving 
					&& _rulesScroll.content.anchoredPosition.x > -(_rulesScroll.content.rect.width - _rulesScroll.viewport.rect.width);
			_prevPageButton.interactable = !_pageIsMoving
			        && _rulesScroll.content.anchoredPosition.x < 0;
			_hideHowToPlayButton.interactable = !_pageIsMoving;
		}

		public void ShowPrevPage() {
			var nextContentXPosition = _rulesScroll.content.anchoredPosition.x + (_rulesScroll.viewport.rect.width + _contentHorizontalGroup.spacing);
			StartCoroutine(MovePage(nextContentXPosition));
		}

		public void ShowNextPage() {
			var nextContentXPosition = _rulesScroll.content.anchoredPosition.x - (_rulesScroll.viewport.rect.width + _contentHorizontalGroup.spacing);
			StartCoroutine(MovePage(nextContentXPosition));
		}

		public void Dispose() {
			_canvasGroup.DOKill();
			_prevPageButton.onClick.RemoveAllListeners();
			_nextPageButton.onClick.RemoveAllListeners();
			_hideHowToPlayButton.onClick.RemoveAllListeners();
		}

		private IEnumerator MovePage(float nextContentXPosition) {
			_pageIsMoving = true;
			UpdateButtonsInteractivity();
			var deltaPosition = new Vector2(_mainMenuConfig.RulesScrollSpeed, 0);
			var pageIsNext = false;
			if (_rulesScroll.content.anchoredPosition.x > nextContentXPosition) {
				deltaPosition *= -1;
				pageIsNext = true;
			}

			while (pageIsNext 
					? _rulesScroll.content.anchoredPosition.x > nextContentXPosition 
					: _rulesScroll.content.anchoredPosition.x < nextContentXPosition) {
				_rulesScroll.content.anchoredPosition += deltaPosition * Time.deltaTime;
				deltaPosition *= 1.2f;
				yield return new WaitForEndOfFrame();
			}
			
			_rulesScroll.content.anchoredPosition 
					= new Vector2(nextContentXPosition, _rulesScroll.content.anchoredPosition.y); 
			_pageIsMoving = false;
			UpdateButtonsInteractivity();
		}

	}

}