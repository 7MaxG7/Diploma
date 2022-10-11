using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;


namespace Infrastructure {

	internal sealed class CreditsView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private ScrollRect _creditsScroll;
		[SerializeField] private Button _closeCreditsButton;

		public event Action OnCloseCreditsClick;
		
		private MainMenuConfig _mainMenuConfig;

		
		public void Init(MainMenuConfig mainMenuConfig) {
			_mainMenuConfig = mainMenuConfig;
			_closeCreditsButton.onClick.AddListener(() => OnCloseCreditsClick?.Invoke());
		}

		public void ShowCredits() {
			gameObject.SetActive(true);
			_canvasGroup.alpha = 0;
			_creditsScroll.content.anchoredPosition = Vector2.zero;
			_canvasGroup.interactable = true;
			_canvasGroup.DOFade(1, _mainMenuConfig.CreditsFadingDuration)
					.OnComplete(() => StartCoroutine(ScrollCredits()));
			
			IEnumerator ScrollCredits() {
				var endContentYPosition = _creditsScroll.content.rect.height - _creditsScroll.viewport.rect.height;
				var creditsScrollDelta = new Vector2(0, _mainMenuConfig.CreditsScrollSpeed);
				while (_creditsScroll.content.anchoredPosition.y < endContentYPosition) {
					_creditsScroll.content.anchoredPosition += creditsScrollDelta * Time.deltaTime;
					yield return new WaitForEndOfFrame();
				}

				HideCredits();
			}
		}

		public void HideCredits() {
			_canvasGroup.interactable = false;
			_canvasGroup.DOKill();
			StopAllCoroutines();
			_canvasGroup.DOFade(0, _mainMenuConfig.CreditsFadingDuration)
					.OnComplete(() => gameObject.SetActive(false));
		}

		public void OnDispose() {
			_canvasGroup.DOKill();
			_closeCreditsButton.onClick.RemoveAllListeners();
		}
	}

}