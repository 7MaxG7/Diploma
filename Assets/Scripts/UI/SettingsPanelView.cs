using UnityEngine;
using UnityEngine.UI;


namespace UI {

	internal class SettingsPanelView : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private Button _closeButton;
		[SerializeField] private Button _leaveGameButton;
		[SerializeField] private Slider _musicVolumeSlider;
		[SerializeField] private Slider _soundVolumeSlider;

		public CanvasGroup CanvasGroup => _canvasGroup;
		public Button CloseButton => _closeButton;
		public Slider MusicVolumeSlider => _musicVolumeSlider;
		public Slider SoundVolumeSlider => _soundVolumeSlider;
		public Button LeaveGameButton => _leaveGameButton;
	}

}