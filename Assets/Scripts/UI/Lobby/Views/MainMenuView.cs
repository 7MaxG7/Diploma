using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure
{
    internal sealed class MainMenuView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _headerLabel;
        [SerializeField] private TMP_Text _scoreLable;
        [SerializeField] private TMP_Text _loginButtonText;
        [SerializeField] private Button _loginPanelButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _howToPlayButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _quitGameButton;
        [Header("Panels")] [SerializeField] private LoginPanelView _loginPanelView;
        [SerializeField] private LobbyScreenView _lobbyScreenView;
        [SerializeField] private CreditsView _creditsView;
        [SerializeField] private HowToPlayView _howToPlayView;

        public event Action OnLoginClick;
        public event Action OnPlayClick;
        public event Action OnSettingsClick;
        public event Action OnHowToPlayClick;
        public event Action OnCreditsClick;
        public event Action OnQuitGameClick;

        // Panels
        public LoginPanelView LoginPanelView => _loginPanelView;
        public LobbyScreenView LobbyScreenView => _lobbyScreenView;
        public CreditsView CreditsView => _creditsView;
        public HowToPlayView HowToPlayView => _howToPlayView;

        private MainMenuConfig _mainMenuConfig;
        

        public void Init(MainMenuConfig mainMenuConfig)
        {
            _mainMenuConfig = mainMenuConfig;
            gameObject.SetActive(true);
            _headerLabel.text = _mainMenuConfig.MainMenuLableText;
            _loginPanelButton.onClick.AddListener(() => OnLoginClick?.Invoke());
            _playButton.onClick.AddListener(() => OnPlayClick?.Invoke());
            _settingsButton.onClick.AddListener(() => OnSettingsClick?.Invoke());
            _howToPlayButton.onClick.AddListener(() => OnHowToPlayClick?.Invoke());
            _creditsButton.onClick.AddListener(() => OnCreditsClick?.Invoke());
            _quitGameButton.onClick.AddListener(() => OnQuitGameClick?.Invoke());
        }

        public void OnDispose()
        {
            _loginPanelButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
            _howToPlayButton.onClick.RemoveAllListeners();
            _creditsButton.onClick.RemoveAllListeners();
            _quitGameButton.onClick.RemoveAllListeners();
        }

        public void UpdateLoginButtons(string userName)
        {
            _loginButtonText.text = string.IsNullOrEmpty(userName) ? _mainMenuConfig.LoginButtonText : userName;
            _loginPanelButton.interactable = string.IsNullOrEmpty(userName);
            _playButton.interactable = !string.IsNullOrEmpty(userName);
        }

        public void SetScore(int? score)
        {
            if (!score.HasValue)
            {
                _scoreLable.gameObject.SetActive(false);
                return;
            }

            _scoreLable.text = string.Format(_mainMenuConfig.ScoreLableTemplate, score.Value);
            _scoreLable.gameObject.SetActive(true);
        }
    }
}