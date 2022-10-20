using UnityEngine;


namespace Infrastructure
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(MainMenuConfig), fileName = nameof(MainMenuConfig), order = 1)]
    internal class MainMenuConfig : ScriptableObject
    {
        [Header("Credits")] [SerializeField] private float _creditsFadingDuration;
        [SerializeField] private float _creditsScrollSpeed;

        [Header("How to play")] [SerializeField]
        private float _rulesFadingDuration;

        [SerializeField] private float _rulesScrollSpeed;
        [Header("Log in")] [SerializeField] private string _confirmLoginAccountButtonText;
        [SerializeField] private string _confirmCreateAccountButtonText;
        [SerializeField] private float _loginPanelFadingDuration;
        [SerializeField] private Color _ordinaryStatusTextColor;
        [SerializeField] private Color _errorStatusTextColor;
        [SerializeField] private Color _successStatusTextColor;
        [SerializeField] private string _loadingStatusPrefixText;
        [SerializeField] private char _loadingStatusSuffixSymbol;
        [SerializeField] private int _minStatusSuffixSymbolsAmount;
        [SerializeField] private int _maxStatusSuffixSymbolsAmount;
        [SerializeField] private string _registrationSuccessStatusText;
        [SerializeField] private string _registrationFailStatusText;
        [SerializeField] private string _loginSuccessStatusText;
        [SerializeField] private string _loginFailStatusText;

        [Tooltip("Time in ms for adding symbol to status suffix")] [SerializeField]
        private int _statusLableSuffixUpdateDelay;

        [Tooltip("Delay in ms before panel hides after success opration")] [SerializeField]
        private int _successfulStatusDelay;

        [Header("Main lobby")] [SerializeField]
        private MainMenuView _mainMenuPref;

        [SerializeField] private LobbyCachedRoomItemView _lobbyCachedRoomItemPref;
        [SerializeField] private RoomPlayerItemView _roomCachedPlayerItemPref;
        [SerializeField] private string _mainMenuLableText;
        [SerializeField] private string _loginButtonText;
        [SerializeField] private string _scoreLableTemplate;
        [SerializeField] private int _scorePerWin;
        [SerializeField] private int _scorePerKill;

        [Header("Games lobby")] [SerializeField]
        private float _lobbyPanelFadingDuration;

        [SerializeField] private Color _unlockedRoomLabelColor;
        [SerializeField] private Color _lockedRoomLabelColor;
        [SerializeField] private string _roomNameTemplate; // Must contain {0} for user name and {1} for time

        public float CreditsFadingDuration => _creditsFadingDuration;
        public float CreditsScrollSpeed => _creditsScrollSpeed;
        public MainMenuView MainMenuPref => _mainMenuPref;
        public LobbyCachedRoomItemView LobbyCachedRoomItemPref => _lobbyCachedRoomItemPref;
        public RoomPlayerItemView RoomCachedPlayerItemPref => _roomCachedPlayerItemPref;
        public string ScoreLableTemplate => _scoreLableTemplate;
        public int ScorePerWin => _scorePerWin;
        public int ScorePerKill => _scorePerKill;
        public float RulesFadingDuration => _rulesFadingDuration;
        public float RulesScrollSpeed => _rulesScrollSpeed;
        public string MainMenuLableText => _mainMenuLableText;
        public string LoginButtonText => _loginButtonText;
        public string ConfirmLoginAccountButtonText => _confirmLoginAccountButtonText;
        public string ConfirmCreateAccountButtonText => _confirmCreateAccountButtonText;
        public float LoginPanelFadingDuration => _loginPanelFadingDuration;
        public int MinStatusSuffixSymbolsAmount => _minStatusSuffixSymbolsAmount;
        public int MaxStatusSuffixSymbolsAmount => _maxStatusSuffixSymbolsAmount;
        public int StatusLableSuffixUpdateDelay => _statusLableSuffixUpdateDelay;
        public Color OrdinaryStatusTextColor => _ordinaryStatusTextColor;
        public string LoadingStatusPrefixText => _loadingStatusPrefixText;
        public char LoadingStatusSuffixSymbol => _loadingStatusSuffixSymbol;
        public int SuccessfulStatusDelay => _successfulStatusDelay;
        public Color ErrorStatusTextColor => _errorStatusTextColor;
        public string RegistrationFailStatusText => _registrationFailStatusText;
        public Color SuccessStatusTextColor => _successStatusTextColor;
        public string LoginSuccessStatusText => _loginSuccessStatusText;
        public string RegistrationSuccessStatusText => _registrationSuccessStatusText;
        public string LoginFailStatusText => _loginFailStatusText;
        public float LobbyPanelFadingDuration => _lobbyPanelFadingDuration;
        public Color UnlockedRoomLabelColor => _unlockedRoomLabelColor;
        public Color LockedRoomLabelColor => _lockedRoomLabelColor;
        public string RoomNameTemplate => _roomNameTemplate;
    }
}