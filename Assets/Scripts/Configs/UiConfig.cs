using UI;
using UnityEngine;


namespace Infrastructure {

	[CreateAssetMenu(menuName = "Configs/" + nameof(UiConfig), fileName = nameof(UiConfig), order = 5)]
	internal class UiConfig : ScriptableObject {
		[SerializeField] private MissionUiView _missionUiView;
		[SerializeField] private int _hpBarAnimationDurationInFrames;
		[SerializeField] private float _canvasFadeAnimationDuration;
		[Header("Skills")]
		[SerializeField] private SkillUiItemView _skillUiItemPrefab;
		[Tooltip("Time after level up before choosing skills ui appears")]
		[SerializeField] private float _skillChooserActivationDelay;
		[Header("Result panel")]
		[SerializeField] private string _winResultText;
		[SerializeField] private Color _winResultColor;
		[SerializeField] private string _looseResultText;
		[SerializeField] private Color _looseResultColor;
		[SerializeField] private float _arrowPointerFadingFrameDelta;

		public MissionUiView MissionUiView => _missionUiView;
		public int HpBarAnimationDurationInFrames => _hpBarAnimationDurationInFrames;
		public SkillUiItemView SkillUiItemPrefab => _skillUiItemPrefab;
		public float CanvasFadeAnimationDuration => _canvasFadeAnimationDuration;
		public float SkillChooserActivationDelay => _skillChooserActivationDelay;
		public string WinResultText => _winResultText;
		public Color WinResultColor => _winResultColor;
		public string LooseResultText => _looseResultText;
		public Color LooseResultColor => _looseResultColor;
		public float ArrowPointerFadingFrameDelta => _arrowPointerFadingFrameDelta;
	}

}