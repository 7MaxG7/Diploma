using System;
using System.Collections;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class PermanentUiController {
		private readonly PermanentUiView _permanentUiView;
		private readonly ICoroutineRunner _coroutineRunner;
		private bool _loadingCurtainIsActivating;
		private bool _loadingCurtainIsDeactivating;
		private bool _loadingCurtainIsActive;

		public PermanentUiController(PermanentUiView permanentUiView, ICoroutineRunner coroutineRunner) {
			_permanentUiView = permanentUiView;
			_coroutineRunner = coroutineRunner;
			
			Object.DontDestroyOnLoad(_permanentUiView.gameObject);
		}

		public void ShowLoadingCurtain(bool showFading = true) {
			_loadingCurtainIsActive = true;
			ShowLoadingStatusAsync();
			
			if (showFading) {
				CheckAndStopLoadingCurtainCoroutines();
				_coroutineRunner.StartCoroutine(ShowLoadingCurtainCoroutine());
			} else
				_permanentUiView.LoadingCurtainCanvasGroup.alpha = 1;
		}

		public void HideLoadingCurtain(bool showFading = true) {
			if (showFading) {
				CheckAndStopLoadingCurtainCoroutines();
				_coroutineRunner.StartCoroutine(HideLoadingCurtainCoroutine());
			} else {
				_permanentUiView.LoadingCurtainCanvasGroup.alpha = 0;
				_loadingCurtainIsActive = false;
			}
		}

		private IEnumerator ShowLoadingCurtainCoroutine() {
			_loadingCurtainIsActivating = true;
			while (_permanentUiView.LoadingCurtainCanvasGroup.alpha < 1) {
				_permanentUiView.LoadingCurtainCanvasGroup.alpha += Math.Min(Constants.LOADING_CURTAIN_FADING_DELTA, _permanentUiView.LoadingCurtainCanvasGroup.alpha);
				yield return new WaitForEndOfFrame();
			}
			_loadingCurtainIsActivating = false;
		}

		private async void ShowLoadingStatusAsync() {
			const int startAwaitingTick = 0;
			const int maxAwaitingTick = 5;
			var currentAwaitingTick = startAwaitingTick;
			while (_loadingCurtainIsActive) {
				_permanentUiView.LoadingCurtainText.text = $"{TextConstants.LOADING_CURTAIN_TEXT_TEMPLATE}{new string('.', currentAwaitingTick)}";
				if (++currentAwaitingTick >= maxAwaitingTick)
					currentAwaitingTick = startAwaitingTick;
				await Task.Delay(250);
			}
		}

		private IEnumerator HideLoadingCurtainCoroutine() {
			_loadingCurtainIsDeactivating = true;
			while (_permanentUiView.LoadingCurtainCanvasGroup.alpha > 0) {
				_permanentUiView.LoadingCurtainCanvasGroup.alpha -= Math.Min(Constants.LOADING_CURTAIN_FADING_DELTA, _permanentUiView.LoadingCurtainCanvasGroup.alpha);
				yield return new WaitForEndOfFrame();
			}
			_loadingCurtainIsActive = false;
			_loadingCurtainIsDeactivating = false;
		}

		private void CheckAndStopLoadingCurtainCoroutines() {
			if (_loadingCurtainIsActivating) {
				_coroutineRunner.StopCoroutine(ShowLoadingCurtainCoroutine());
				_loadingCurtainIsActivating = false;
			}
			else if (_loadingCurtainIsDeactivating) {
				_coroutineRunner.StopCoroutine(HideLoadingCurtainCoroutine());
				_loadingCurtainIsDeactivating = false;
			}
		}
	}

}