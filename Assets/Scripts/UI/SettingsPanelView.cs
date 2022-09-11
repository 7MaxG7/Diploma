using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace UI {

	internal sealed class SettingsPanelView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private Button _closeButton;
		[SerializeField] private Button _leaveGameButton;
		[SerializeField] private Slider _musicVolumeSlider;
		[SerializeField] private Slider _soundVolumeSlider;
		[SerializeField] private GameObject _missionSettingsSection;

		public event Action<float> OnMusicVolumeSliderValueChange;
		public event Action<float> OnSoundVolumeSliderValueChange;
		public event Action OnLeaveGameClick;
		public event Action OnCloseSettingsClick;
		

		public void Init() {
			_musicVolumeSlider.onValueChanged.AddListener(value => OnMusicVolumeSliderValueChange?.Invoke(value));
			_soundVolumeSlider.onValueChanged.AddListener(value => OnSoundVolumeSliderValueChange?.Invoke(value));
			_leaveGameButton.onClick.AddListener(() => OnLeaveGameClick?.Invoke());
			_closeButton.onClick.AddListener(() => OnCloseSettingsClick?.Invoke());
			_canvasGroup.alpha = 0;
		}

		public void SetVolumeSliders(float musicVolume, float soundVolume) {
			_musicVolumeSlider.value = musicVolume;
			_soundVolumeSlider.value = soundVolume;
		}

		public void ShowSettingsPanel(bool missionSettingsSectionIsActive, float animationDuration) {
			_missionSettingsSection.SetActive(missionSettingsSectionIsActive);
			_canvasGroup.DOKill();
			gameObject.SetActive(true);
			_canvasGroup.DOFade(1, animationDuration);
		}

		public void HideSettingsPanel(float animationDuration) {
			_canvasGroup.DOKill();
			if (animationDuration > 0)
				_canvasGroup.DOFade(0, animationDuration)
						.OnComplete(() => gameObject.SetActive(false));
			else
				gameObject.SetActive(false);
		}

		public void Dispose() {
			_musicVolumeSlider.onValueChanged.RemoveAllListeners();
			_soundVolumeSlider.onValueChanged.RemoveAllListeners();
			_leaveGameButton.onClick.RemoveAllListeners();
			_closeButton.onClick.RemoveAllListeners();
		}
	}

}