using UnityEngine;


namespace UI {

	internal class MissionUiView : MonoBehaviour {
		[SerializeField] private PlayerPanelView _playerPanel;
		[SerializeField] private SkillsPanelView _skillsPanel;

		public PlayerPanelView PlayerPanel => _playerPanel;
		public SkillsPanelView SkillsPanel => _skillsPanel;
	}

}