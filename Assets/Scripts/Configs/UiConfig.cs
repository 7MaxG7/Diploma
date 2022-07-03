using UI;
using UnityEngine;


namespace Infrastructure {

	[CreateAssetMenu(menuName = "Configs/" + nameof(UiConfig), fileName = nameof(UiConfig), order = 5)]
	internal class UiConfig : ScriptableObject {
		[SerializeField] private MissionUiView _missionUiView;
		[SerializeField] private int _hpBarAnimationDurationInFrames;

		public MissionUiView MissionUiView => _missionUiView;
		public int HpBarAnimationDurationInFrames => _hpBarAnimationDurationInFrames;
	}

}