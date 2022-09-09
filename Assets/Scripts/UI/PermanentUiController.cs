using System;
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
		public bool IsActivating => _permanentUiView.CurtainIsActivating;
		/// <summary>
		/// Флаг, что шторка активна (анимации завершены)
		/// </summary>
		public bool CurtainIsActive => _permanentUiView.CurtainIsActive;

		private readonly IPermanentUiView _permanentUiView;
		private readonly ISoundManager _soundManager;
		private readonly UiConfig _uiConfig;
		private ICoroutineRunner _coroutineRunner;


		[Inject]
		public PermanentUiController(IPermanentUiView permanentUiView, ISoundManager soundManager, UiConfig uiConfig) {
			_permanentUiView = permanentUiView;
			_soundManager = soundManager;
			_uiConfig = uiConfig;
		}

		public void Dispose() {
			_permanentUiView.SettingsPanel.OnMusicVolumeSliderValueChange -= _soundManager.SetMusicVolume;
			_permanentUiView.SettingsPanel.OnSoundVolumeSliderValueChange -= _soundManager.SetSoundVolume;
			_permanentUiView.SettingsPanel.OnLeaveGameClick -= LeaveGame;
			_permanentUiView.SettingsPanel.OnCloseSettingsClick -= HideSettingsPanel;
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
				_permanentUiView.SettingsPanel.SetVolumeSliders(_soundManager.GetMusicVolume(), _soundManager.GetSoundVolume());
				_permanentUiView.SettingsPanel.OnMusicVolumeSliderValueChange += _soundManager.SetMusicVolume;
				_permanentUiView.SettingsPanel.OnSoundVolumeSliderValueChange += _soundManager.SetSoundVolume;
				_permanentUiView.SettingsPanel.OnLeaveGameClick += LeaveGame;
				_permanentUiView.SettingsPanel.OnCloseSettingsClick += HideSettingsPanel;
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

		private void HideSettingsPanel() {
			PlayerPrefs.SetFloat(TextConstants.MUSIC_VOLUME_PREFS_KEY, _soundManager.GetMusicVolume());
			PlayerPrefs.SetFloat(TextConstants.SOUND_VOLUME_PREFS_KEY, _soundManager.GetSoundVolume());
			PlayerPrefs.Save();
			_permanentUiView.SettingsPanel.HideSettingsPanel(_uiConfig.CanvasFadeAnimationDuration);
		}
	}

}