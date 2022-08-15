using UnityEngine;
using UnityEngine.UI;


namespace UI {

	internal class MissionUiView : MonoBehaviour {
		[SerializeField] private PlayerPanelView _playerPanel;
		[SerializeField] private SkillsPanelView _skillsPanel;
		[SerializeField] private Button _settingsButton;
		[SerializeField] private CanvasGroup _compassPointerCanvasGroup;

		public PlayerPanelView PlayerPanel => _playerPanel;
		public SkillsPanelView SkillsPanel => _skillsPanel;
		public Button SettingsButton => _settingsButton;
		public CanvasGroup CompassPointerCanvasGroup => _compassPointerCanvasGroup;
	}

}