using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Infrastructure
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(UiConfig), fileName = nameof(UiConfig), order = 5)]
    internal class UiConfig : ScriptableObject
    {
        [SerializeField] private AssetReferenceGameObject _missionUiView;
        [SerializeField] private float _canvasFadeAnimationDuration;
        [Header("Curtain")] [SerializeField] private string _loadingStatusPrefixText;
        [SerializeField] private char _loadingStatusSuffixSymbol;
        [SerializeField] private int _minSuffixSymbolsAmount;
        [SerializeField] private int _maxSuffixSymbolsAmount;
        [Tooltip("Time in ms for adding symbol to status suffix")] [SerializeField]
        private int _statusLableSuffixUpdateDelay;
        [SerializeField] private float _curtainFadingAlfaPerFrameDelta;
        
        [Header("Skills")]
        [SerializeField] private AssetReferenceGameObject _skillUiItemPrefab;
        [Tooltip("Time after level up before choosing skills ui appears")] [SerializeField]
        private float _skillChooserActivationDelay;

        [Header("Result panel")]
        [SerializeField] private string _winResultText;

        [SerializeField] private Color _winResultColor;
        [SerializeField] private string _looseResultText;
        [SerializeField] private Color _looseResultColor;
        [SerializeField] private float _arrowPointerFadingFrameDelta;

        [Header("Players ui")]
        [SerializeField] private int _hpBarAnimationDurationInFrames;
        [SerializeField] private string _healthBarTextTemplate; // Must contain {0} for current hp and {1} for max hp
        [SerializeField] private string _experienceBarLevelTextTemplate; // Must contain {0} for current level
        [SerializeField] private string _newWeaponLevelText;
        [SerializeField] private string _upgradeWeaponLevelText; // Must contain {0} for weapon nex level

        public AssetReferenceGameObject MissionUiView => _missionUiView;
        public int HpBarAnimationDurationInFrames => _hpBarAnimationDurationInFrames;
        public AssetReferenceGameObject SkillUiItemPrefab => _skillUiItemPrefab;
        public float CanvasFadeAnimationDuration => _canvasFadeAnimationDuration;
        public float SkillChooserActivationDelay => _skillChooserActivationDelay;
        public string WinResultText => _winResultText;
        public Color WinResultColor => _winResultColor;
        public string LooseResultText => _looseResultText;
        public Color LooseResultColor => _looseResultColor;
        public float ArrowPointerFadingFrameDelta => _arrowPointerFadingFrameDelta;
        public string LoadingStatusPrefixText => _loadingStatusPrefixText;
        public char LoadingStatusSuffixSymbol => _loadingStatusSuffixSymbol;
        public int MinSuffixSymbolsAmount => _minSuffixSymbolsAmount;
        public int MaxSuffixSymbolsAmount => _maxSuffixSymbolsAmount;
        public float CurtainFadingAlfaPerFrameDelta => _curtainFadingAlfaPerFrameDelta;
        public int StatusLableSuffixUpdateDelay => _statusLableSuffixUpdateDelay;
        public string HealthBarTextTemplate => _healthBarTextTemplate;
        public string ExperienceBarLevelTextTemplate => _experienceBarLevelTextTemplate;
        public string NewWeaponLevelText => _newWeaponLevelText;
        public string UpgradeWeaponLevelText => _upgradeWeaponLevelText;
    }
}