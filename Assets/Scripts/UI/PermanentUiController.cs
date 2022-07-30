using System;
using System.Collections;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class PermanentUiController : IPermanentUiController, IDisposable {

		public event Action OnCurtainShown;
		public bool IsActivating => _loadingCurtainIsActivating;
		public bool IsActive => _loadingCurtainIsActive;
		
		private readonly IPermanentUiView _permanentUiView;
		private readonly ISoundController _soundController;
		private ICoroutineRunner _coroutineRunner;
		private bool _loadingCurtainIsActivating;
		private bool _loadingCurtainIsDeactivating;
		private bool _loadingCurtainIsActive;
		private bool IsFading => _loadingCurtainIsActivating || _loadingCurtainIsDeactivating;


		[Inject]
		public PermanentUiController(IPermanentUiView permanentUiView, ISoundController soundController) {
			_permanentUiView = permanentUiView;
			_soundController = soundController;
		}
		
		public void Dispose() {
			_permanentUiView.SettingsPanel.MusicVolumeSlider.onValueChanged.RemoveAllListeners();
			_permanentUiView.SettingsPanel.SoundVolumeSlider.onValueChanged.RemoveAllListeners();
			_permanentUiView.SettingsPanel.CloseButton.onClick.RemoveAllListeners();
		}

		public void Init(ICoroutineRunner coroutineRunner) {
			_coroutineRunner = coroutineRunner;
			InitSettingsPanel();
			Object.DontDestroyOnLoad(_permanentUiView.GameObject);

			
			void InitSettingsPanel() {
				_permanentUiView.SettingsPanel.MusicVolumeSlider.value = _soundController.MusicVolume;
				_permanentUiView.SettingsPanel.SoundVolumeSlider.value = _soundController.SoundVolume;
				_permanentUiView.SettingsPanel.MusicVolumeSlider.onValueChanged.AddListener(volume => _soundController.MusicVolume = volume);
				_permanentUiView.SettingsPanel.SoundVolumeSlider.onValueChanged.AddListener(volume => _soundController.SoundVolume = volume);
				_permanentUiView.SettingsPanel.CloseButton.onClick.AddListener(HideSettingsPanel);
				_permanentUiView.SettingsPanel.gameObject.SetActive(false);
			}
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

		public void ShowSettingsPanel() {
			_permanentUiView.SettingsPanel.gameObject.SetActive(true);
		}

		private void HideSettingsPanel() {
			_permanentUiView.SettingsPanel.gameObject.SetActive(false);
			PlayerPrefs.SetFloat(TextConstants.MUSIC_VOLUME_PREFS_KEY, _permanentUiView.SettingsPanel.MusicVolumeSlider.value);
			PlayerPrefs.SetFloat(TextConstants.SOUND_VOLUME_PREFS_KEY, _permanentUiView.SettingsPanel.SoundVolumeSlider.value);
			PlayerPrefs.Save();
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
			while (IsActive) {
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
			} else if (_loadingCurtainIsDeactivating) {
				_coroutineRunner.StopCoroutine(HideLoadingCurtainCoroutine());
				_loadingCurtainIsDeactivating = false;
			}
		}
	}

}