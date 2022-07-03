using UnityEngine;


namespace UI {

	internal class MissionUiView : MonoBehaviour {
		[SerializeField] private PlayerPanelView _playerPanel;

		public PlayerPanelView PlayerPanel => _playerPanel;
	}

}