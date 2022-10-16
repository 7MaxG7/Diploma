using System;
using System.Linq;
using Services;
using Sounds;
using UI;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal sealed class PermanentUiController : IPermanentUiController, IDisposable {
		public event Action OnCurtainShown;
		public event  Action OnLeaveGameClicked;
		public event  Action OnResultPanelClosed;
		public bool IsActivating => _permanentUiView.CurtainIsActivating;
		/// <summary>
		/// Флаг, что шторка активна (анимации завершены)
		/// </summary>
		public bool CurtainIsActive => _permanentUiView.CurtainIsActive;

		private readonly IPermanentUiView _permanentUiView;
		private readonly ISoundController _soundController;
		private readonly IPlayerPrefsService _playerPrefsService;
		private readonly IScreenService _screenService;
		private readonly UiConfig _uiConfig;
		private ICoroutineRunner _coroutineRunner;


		[Inject]
		public PermanentUiController(IPermanentUiView permanentUiView, ISoundController soundController
			, IPlayerPrefsService playerPrefsService, IScreenService screenService, UiConfig uiConfig) {
			_permanentUiView = permanentUiView;
			_soundController = soundController;
			_playerPrefsService = playerPrefsService;
			_screenService = screenService;
			_uiConfig = uiConfig;
		}

		public void Dispose() {
			_permanentUiView.SettingsPanel.OnMusicVolumeSliderValueChange -= _soundController.SetMusicVolume;
			_permanentUiView.SettingsPanel.OnSoundVolumeSliderValueChange -= _soundController.SetSoundVolume;
			_permanentUiView.SettingsPanel.OnLeaveGameClick -= LeaveGame;
			_permanentUiView.SettingsPanel.OnCloseSettingsClick -= HideSettingsPanel;
#if UNITY_EDITOR || UNITY_STANDALONE
			_permanentUiView.SettingsPanel.OnResolutionChanged -= SetResolutionByIndex;
			_permanentUiView.SettingsPanel.OnWindowModeChanged -= SetWindowMode;
#endif
			_permanentUiView.ResultPanel.OnCloseResultPanelClick -= HideMissionResult;
			_permanentUiView.OnCurtainShown -= InvokeOnCurtainShown;
			_permanentUiView.SettingsPanel.Dispose();
			_permanentUiView.ResultPanel.Dispose();
			_permanentUiView.Dispose();
		}

		public void Init(ICoroutineRunner coroutineRunner) {
			_coroutineRunner = coroutineRunner;
			_permanentUiView.Init(_coroutineRunner, _uiConfig);
			InitCurtain();
			InitSettingsPanel();
			InitResultPanel();
			Object.DontDestroyOnLoad(_permanentUiView.GameObject);

			void InitCurtain() {
				_permanentUiView.OnCurtainShown += InvokeOnCurtainShown;
			}

			void InitSettingsPanel() {
				_permanentUiView.SettingsPanel.Init();
				_permanentUiView.SettingsPanel.SetVolumeSliders(_soundController.GetMusicVolume(), _soundController.GetSoundVolume());
				_permanentUiView.SettingsPanel.OnMusicVolumeSliderValueChange += _soundController.SetMusicVolume;
				_permanentUiView.SettingsPanel.OnSoundVolumeSliderValueChange += _soundController.SetSoundVolume;
				_permanentUiView.SettingsPanel.OnLeaveGameClick += LeaveGame;
				_permanentUiView.SettingsPanel.OnCloseSettingsClick += HideSettingsPanel;
#if UNITY_EDITOR || UNITY_STANDALONE
				_permanentUiView.SettingsPanel.ToggleStandaloneSettings(true);
				_permanentUiView.SettingsPanel.SetupResolutionOptions(Screen.resolutions);
				_permanentUiView.SettingsPanel.OnResolutionChanged += SetResolutionByIndex;
				_permanentUiView.SettingsPanel.OnWindowModeChanged += SetWindowMode;
				SetSavedResolution();
#else
				_permanentUiView.SettingsPanel.ToggleStandaloneSettings(false);
#endif
				_permanentUiView.SettingsPanel.HideSettingsPanel(0);
			}

			void InitResultPanel() {
				_permanentUiView.ResultPanel.Init(_uiConfig);
				_permanentUiView.ResultPanel.OnCloseResultPanelClick += HideMissionResult;
				_permanentUiView.ResultPanel.HideMissionResult();
			}
		}

		public void ShowLoadingCurtain(bool isAnimated = true) {
			if (_permanentUiView.CurtainIsActive || _permanentUiView.CurtainIsActivating)
				return;
			
			_permanentUiView.StopLoadingCurtainAnimation();
			_permanentUiView.ShowLoadingCurtain(isAnimated);
			_permanentUiView.ShowLoadingStatusLableAsync();
		}

		public void HideLoadingCurtain(bool animationIsOn = true, bool interruptCurrentAnimation = false) {
			if (interruptCurrentAnimation)
				_permanentUiView.StopLoadingCurtainAnimation();
			_permanentUiView.HideLoadingCurtain(animationIsOn);
			_permanentUiView.ShowLoadingStatusLableAsync();
		}

		public void ShowSettingsPanel(bool missionSettingsSectionIsActive = false) {
			_permanentUiView.SettingsPanel.ShowSettingsPanel(missionSettingsSectionIsActive, _uiConfig.CanvasFadeAnimationDuration);
		}

		public void ShowMissionResult(MissionEndInfo missionEndInfo) {
			_permanentUiView.ResultPanel.ShowMissionResult(missionEndInfo.IsWinner, missionEndInfo.KillsAmount, _uiConfig.CanvasFadeAnimationDuration);
		}

		private void HideMissionResult() {
			OnCurtainShown += HideResultPanel;
			ShowLoadingCurtain();

			void HideResultPanel() {
				OnCurtainShown -= HideResultPanel;
				_permanentUiView.ResultPanel.HideMissionResult();
				OnResultPanelClosed?.Invoke();
			}
		}

		private void InvokeOnCurtainShown() {
			OnCurtainShown?.Invoke();
		}

		private void LeaveGame() {
			HideSettingsPanel();
			OnLeaveGameClicked?.Invoke();
		}

		private void HideSettingsPanel()
		{
			_playerPrefsService.SaveSoundVolumes(_soundController.GetMusicVolume(), _soundController.GetSoundVolume());
#if UNITY_EDITOR || UNITY_STANDALONE
			_playerPrefsService.SaveCurrentResolution(_screenService.GetCurrentResolution());
			_playerPrefsService.SaveCurrentFullScreenMode(_screenService.GetCurrentFullScreenMode());
#endif
			_permanentUiView.SettingsPanel.HideSettingsPanel(_uiConfig.CanvasFadeAnimationDuration);
		}
		
#if UNITY_EDITOR || UNITY_STANDALONE
		private void SetResolutionByIndex(int resolutionIndex)
		{
			SetResolution(resolutionIndex, null);
		}

		private void SetWindowMode(bool isFullScreen)
		{
			SetResolution(null, isFullScreen);
		}

		private void SetResolution(int? resolutionIndex, bool? isFullScreen)
		{
			isFullScreen ??= Screen.fullScreen;
			var resolution = resolutionIndex.HasValue
				? Screen.resolutions[resolutionIndex.Value]
				: _screenService.GetCurrentResolution();

			Screen.SetResolution(resolution.width, resolution.height, isFullScreen.Value, resolution.refreshRate);
		}

		private void SetSavedResolution()
		{
			var resolution = _playerPrefsService.GetSavedResolution() ?? _screenService.GetCurrentResolution();
			var isFullScreen = _playerPrefsService.GetFullScreenMode() ?? _screenService.GetCurrentFullScreenMode();

			var resolutionItem = Screen.resolutions.FirstOrDefault(item => item.Equals(resolution));
			var resolutionIndex = Array.IndexOf(Screen.resolutions, resolutionItem);
			if (resolutionIndex < 0)
				return;

			SetResolution(resolutionIndex, isFullScreen);
			_permanentUiView.SettingsPanel.SetResolutionUiElements(resolutionIndex, isFullScreen);
		}
#endif
	}
}