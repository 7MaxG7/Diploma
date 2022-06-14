using System;
using System.Collections;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class PermanentUiController : IPermanentUiController {

		public event Action OnCurtainShown;
		public bool IsActivating => _loadingCurtainIsActivating;
		
		private bool IsFading => _loadingCurtainIsActivating || _loadingCurtainIsDeactivating;
		private readonly IPermanentUiView _permanentUiView;
		private ICoroutineRunner _coroutineRunner;
		private bool _loadingCurtainIsActivating;
		private bool _loadingCurtainIsDeactivating;
		private bool _loadingCurtainIsActive;


		[Inject]
		public PermanentUiController(IPermanentUiView permanentUiView) {
			_permanentUiView = permanentUiView;
		}
		
		public void Init(ICoroutineRunner coroutineRunner) {
			_coroutineRunner = coroutineRunner;
			
			Object.DontDestroyOnLoad(_permanentUiView.GameObject);
		}

		public void ShowLoadingCurtain(bool animationIsOn = true, bool isForced = false) {
			_loadingCurtainIsActive = true;
			ShowLoadingStatusAsync();
			
			if (animationIsOn) {
				if (!isForced)
					CheckAndStopLoadingCurtainCoroutines();
				_coroutineRunner.StartCoroutine(ShowLoadingCurtainCoroutine());
			} else
				_permanentUiView.LoadingCurtainCanvasGroup.alpha = 1;
		}

		public void HideLoadingCurtain(bool animationIsOn = true, bool isForced = false) {
			if (animationIsOn) {
				if (!isForced)
					CheckAndStopLoadingCurtainCoroutines();
				_coroutineRunner.StartCoroutine(HideLoadingCurtainCoroutine());
			} else {
				_permanentUiView.LoadingCurtainCanvasGroup.alpha = 0;
				_loadingCurtainIsActive = false;
			}
		}

		private IEnumerator ShowLoadingCurtainCoroutine() {
			while (IsFading) {
				yield return new WaitForEndOfFrame();
			}
			
			_loadingCurtainIsActivating = true;
			while (_permanentUiView.LoadingCurtainCanvasGroup.alpha < 1) {
				_permanentUiView.LoadingCurtainCanvasGroup.alpha += Math.Min(Constants.LOADING_CURTAIN_FADING_DELTA, 1 - _permanentUiView.LoadingCurtainCanvasGroup.alpha);
				yield return new WaitForEndOfFrame();
			}
			OnCurtainShown?.Invoke();
			_permanentUiView.LoadingCurtainCanvasGroup.blocksRaycasts = true;
			_loadingCurtainIsActivating = false;
		}
		
		private IEnumerator HideLoadingCurtainCoroutine() {
			while (IsFading) {
				yield return new WaitForEndOfFrame();
			}
			
			_loadingCurtainIsDeactivating = true;
			_permanentUiView.LoadingCurtainCanvasGroup.blocksRaycasts = false;
			while (_permanentUiView.LoadingCurtainCanvasGroup.alpha > 0) {
				_permanentUiView.LoadingCurtainCanvasGroup.alpha -= Math.Min(Constants.LOADING_CURTAIN_FADING_DELTA, _permanentUiView.LoadingCurtainCanvasGroup.alpha);
				yield return new WaitForEndOfFrame();
			}
			_loadingCurtainIsActive = false;
			_loadingCurtainIsDeactivating = false;
		}

		private async void ShowLoadingStatusAsync() {
			const int startAwaitingTick = 0;
			const int maxAwaitingTick = 5;
			var currentAwaitingTick = startAwaitingTick;
			while (_loadingCurtainIsActive) {
				_permanentUiView.LoadingCurtainText.text = $"{TextConstants.LOADING_STATUS_TEXT_TEMPLATE}{new string('.', currentAwaitingTick)}";
				if (++currentAwaitingTick >= maxAwaitingTick)
					currentAwaitingTick = startAwaitingTick;
				await Task.Delay(250);
			}
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