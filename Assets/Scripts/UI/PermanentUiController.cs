using System;
using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using UI;
using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class PermanentUiController : IPermanentUiController, IDisposable {

		public event Action OnCurtainShown;
		public event  Action OnLeaveGameClicked;
		public event  Action OnResultPanelClosed;
		public bool IsActivating => _loadingCurtainIsActivating;
		public bool IsActive => _loadingCurtainIsActive;
		
		private readonly IPermanentUiView _permanentUiView;
		private readonly ISoundController _soundController;
		private readonly UiConfig _uiConfig;
		private ICoroutineRunner _coroutineRunner;
		private bool _loadingCurtainIsActivating;
		private bool _loadingCurtainIsDeactivating;
		private bool _loadingCurtainIsActive;
		private bool IsFading => _loadingCurtainIsActivating || _loadingCurtainIsDeactivating;


		[Inject]
		public PermanentUiController(IPermanentUiView permanentUiView, ISoundController soundController, UiConfig uiConfig) {
			_permanentUiView = permanentUiView;
			_soundController = soundController;
			_uiConfig = uiConfig;
		}
		
		public void Dispose() {
			_permanentUiView.SettingsPanel.MusicVolumeSlider.onValueChanged.RemoveAllListeners();
			_permanentUiView.SettingsPanel.SoundVolumeSlider.onValueChanged.RemoveAllListeners();
			_permanentUiView.SettingsPanel.LeaveGameButton.onClick.RemoveAllListeners();
			_permanentUiView.SettingsPanel.CloseButton.onClick.RemoveAllListeners();
			_permanentUiView.ResultPanel.ClosePanelButton.onClick.RemoveAllListeners();
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
				_permanentUiView.SettingsPanel.LeaveGameButton.onClick.AddListener(LeaveGame);
				_permanentUiView.SettingsPanel.CloseButton.onClick.AddListener(HideSettingsPanel);
				_permanentUiView.SettingsPanel.gameObject.SetActive(false);
				_permanentUiView.ResultPanel.ClosePanelButton.onClick.AddListener(HideMissionResult);
				_permanentUiView.ResultPanel.gameObject.SetActive(false);
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

		public void ShowSettingsPanel(bool missionPanelIsActive = false) {
			_permanentUiView.MissionSettingsPanel.SetActive(missionPanelIsActive);
			_permanentUiView.SettingsPanel.CanvasGroup.DOKill();
			_permanentUiView.SettingsPanel.gameObject.SetActive(true);
			_permanentUiView.SettingsPanel.CanvasGroup.DOFade(1, _uiConfig.CanvasFadeAnimationDuration);
		}

		public void ShowMissionResult(MissionEndInfo missionEndInfo) {
			_permanentUiView.ResultPanel.gameObject.SetActive(true);
			_permanentUiView.ResultPanel.ResultLableText.text = missionEndInfo.IsWinner ? _uiConfig.WinResultText : _uiConfig.LooseResultText;
			_permanentUiView.ResultPanel.ResultLableText.color = missionEndInfo.IsWinner ? _uiConfig.WinResultColor : _uiConfig.LooseResultColor;
			_permanentUiView.ResultPanel.KillsAmount.text = missionEndInfo.KillsAmount.ToString();
			_permanentUiView.ResultPanel.CanvasGroup.alpha = 0;
			_permanentUiView.ResultPanel.CanvasGroup.DOFade(1, _uiConfig.CanvasFadeAnimationDuration);
		}

		private void HideMissionResult() {
			OnCurtainShown += HideResultPanel;
			ShowLoadingCurtain();


			void HideResultPanel() {
				OnCurtainShown -= HideResultPanel;
				_permanentUiView.ResultPanel.CanvasGroup.DOKill();
				_permanentUiView.ResultPanel.gameObject.SetActive(false);
				OnResultPanelClosed?.Invoke();
			}
		}

		private void LeaveGame() {
			HideSettingsPanel();
			OnLeaveGameClicked?.Invoke();
		}

		private void HideSettingsPanel() {
			PlayerPrefs.SetFloat(TextConstants.MUSIC_VOLUME_PREFS_KEY, _permanentUiView.SettingsPanel.MusicVolumeSlider.value);
			PlayerPrefs.SetFloat(TextConstants.SOUND_VOLUME_PREFS_KEY, _permanentUiView.SettingsPanel.SoundVolumeSlider.value);
			PlayerPrefs.Save();
			_permanentUiView.SettingsPanel.CanvasGroup.DOKill();
			_permanentUiView.SettingsPanel.CanvasGroup.DOFade(0, _uiConfig.CanvasFadeAnimationDuration)
					.OnComplete(() => _permanentUiView.SettingsPanel.gameObject.SetActive(false));
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