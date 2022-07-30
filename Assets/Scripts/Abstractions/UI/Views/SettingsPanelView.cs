using UnityEngine;
using UnityEngine.UI;


namespace UI {

	internal class SettingsPanelView : MonoBehaviour {
		[SerializeField] private Button _closeButton;
		[SerializeField] private Slider _musicVolumeSlider;
		[SerializeField] private Slider _soundVolumeSlider;

		public Button CloseButton => _closeButton;
		public Slider MusicVolumeSlider => _musicVolumeSlider;
		public Slider SoundVolumeSlider => _soundVolumeSlider;
	}

}