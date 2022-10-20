using System.Collections.Generic;
using PlayFab;
using Services;
using UI;
using UnityEngine;
using Utils;
using Zenject;


namespace Infrastructure
{
    internal sealed class MainMenuController : IMainMenuController
    {
        // public event Action<string> OnPlayfabLogin;

        private string _userName;
        private string _playfabId;
        private MainMenuView _mainMenuView;
        private LoginPanelController _loginPanelController;
        private LobbyScreenController _lobbyScreenController;
        private readonly IMissionResultManager _missionResultManager;
        private readonly IPermanentUiController _permanentUiController;
        private readonly IViewsFactory _viewsFactory;
        private readonly IPhotonManager _photonManager;
        private readonly IPlayfabManager _playfabManager;
        private readonly MainMenuConfig _mainMenuConfig;
        private bool _rulesPageIsMoving;
        private bool _isSetuped;


        [Inject]
        public MainMenuController(IMissionResultManager missionResultManager,
            IPermanentUiController permanentUiController
            , IViewsFactory viewsFactory, IPhotonManager photonManager, IPlayfabManager playfabManager,
            MainMenuConfig mainMenuConfig)
        {
            _missionResultManager = missionResultManager;
            _viewsFactory = viewsFactory;
            _photonManager = photonManager;
            _playfabManager = playfabManager;
            _permanentUiController = permanentUiController;
            _mainMenuConfig = mainMenuConfig;
        }

        public void Dispose()
        {
            OnDispose();
        }

        public void OnDispose()
        {
            if (!_isSetuped)
                return;

            _isSetuped = false;
            _mainMenuView.OnLoginClick -= _loginPanelController.ShowPanel;
            _mainMenuView.OnPlayClick -= _lobbyScreenController.ShowScreen;
            _mainMenuView.OnSettingsClick -= OpenSettingsPanel;
            _mainMenuView.OnHowToPlayClick -= _mainMenuView.HowToPlayView.ShowRules;
            _mainMenuView.HowToPlayView.OnPrevPageClick -= _mainMenuView.HowToPlayView.ShowPrevPage;
            _mainMenuView.HowToPlayView.OnNextPageClick -= _mainMenuView.HowToPlayView.ShowNextPage;
            _mainMenuView.HowToPlayView.OnHidePanelClick -= _mainMenuView.HowToPlayView.HideRules;
            _mainMenuView.OnCreditsClick -= _mainMenuView.CreditsView.ShowCredits;
            _mainMenuView.CreditsView.OnCloseCreditsClick -= _mainMenuView.CreditsView.HideCredits;
            _mainMenuView.OnQuitGameClick -= QuitGame;
            _loginPanelController.OnUserLoginSuccess -= SetUser;
            _loginPanelController.OnDispose();
            _lobbyScreenController.OnDispose();
            _mainMenuView.HowToPlayView.OnDispose();
            _mainMenuView.CreditsView.OnDispose();
            _mainMenuView.OnDispose();
        }


        public void SetupMainMenu()
        {
            if (_mainMenuView == null)
            {
                _mainMenuView = _viewsFactory.CreateMainMenu();
            }

            _mainMenuView.Init(_mainMenuConfig);
            InitHowToPlay();
            InitCredits();
            InitLoginPanel();
            InitLobbyPanel();
            InitButtons();
            SetupScoresLable();
            _isSetuped = true;

            void InitHowToPlay()
            {
                _mainMenuView.HowToPlayView.Init(_mainMenuConfig);
                _mainMenuView.HowToPlayView.OnPrevPageClick += _mainMenuView.HowToPlayView.ShowPrevPage;
                _mainMenuView.HowToPlayView.OnNextPageClick += _mainMenuView.HowToPlayView.ShowNextPage;
                _mainMenuView.HowToPlayView.OnHidePanelClick += _mainMenuView.HowToPlayView.HideRules;
            }

            void InitCredits()
            {
                _mainMenuView.CreditsView.Init(_mainMenuConfig);
                _mainMenuView.CreditsView.OnCloseCreditsClick += _mainMenuView.CreditsView.HideCredits;
            }

            void InitLoginPanel()
            {
                _loginPanelController =
                    new LoginPanelController(_playfabManager, _mainMenuView.LoginPanelView, _mainMenuConfig);
                _loginPanelController.Init();
                _loginPanelController.OnUserLoginSuccess += SetUser;
            }

            void InitLobbyPanel()
            {
                _lobbyScreenController = new LobbyScreenController(_photonManager, _mainMenuView.LobbyScreenView,
                    _mainMenuConfig, _permanentUiController);
                if (_playfabManager.CheckClientAutorization())
                {
                    _lobbyScreenController.Init(_userName);
                }
            }

            void InitButtons()
            {
                _mainMenuView.OnLoginClick += _loginPanelController.ShowPanel;
                _mainMenuView.OnPlayClick += _lobbyScreenController.ShowScreen;
                _mainMenuView.OnSettingsClick += OpenSettingsPanel;
                _mainMenuView.OnHowToPlayClick += _mainMenuView.HowToPlayView.ShowRules;
                _mainMenuView.OnCreditsClick += _mainMenuView.CreditsView.ShowCredits;
                _mainMenuView.OnQuitGameClick += QuitGame;
                _mainMenuView.UpdateLoginButtons(_userName);
            }
        }

        private void OpenSettingsPanel()
        {
            _permanentUiController.ShowSettingsPanel();
        }

        private void QuitGame()
        {
            OnDispose();
            _photonManager.Disconnect();
            Application.Quit();
        }

        private void SetUser(string userName, string playfabId)
        {
            // OnPlayfabLogin?.Invoke(playfabId);
            _userName = userName;
            _playfabId = playfabId;
            SetupScoresLable();
            _lobbyScreenController.Init(_userName);
            _mainMenuView.UpdateLoginButtons(_userName);
        }

        private void SetupScoresLable()
        {
            if (string.IsNullOrEmpty(_playfabId))
            {
                _mainMenuView.SetScore(null);
                return;
            }

            _playfabManager.GetData(_playfabId, ParseData, LogError, Constants.WINS_AMOUNT_PLAYFAB_KEY,
                Constants.KILLS_AMOUNT_PLAYFAB_KEY);

            void ParseData(Dictionary<string, string> data)
            {
                var winsScores = 0;
                var killsScores = 0;
                if (data.ContainsKey(Constants.WINS_AMOUNT_PLAYFAB_KEY) &&
                    int.TryParse(data[Constants.WINS_AMOUNT_PLAYFAB_KEY], out var winsAmount))
                {
                    _missionResultManager.SetWinsAmount(winsAmount);
                    winsScores = winsAmount * _mainMenuConfig.ScorePerWin;
                }

                if (data.ContainsKey(Constants.KILLS_AMOUNT_PLAYFAB_KEY) &&
                    int.TryParse(data[Constants.KILLS_AMOUNT_PLAYFAB_KEY], out var killsAmount))
                {
                    _missionResultManager.SetKillsAmount(killsAmount);
                    killsScores = killsAmount * _mainMenuConfig.ScorePerKill;
                }

                _mainMenuView.SetScore(winsScores + killsScores);
            }

            void LogError(PlayFabError error)
            {
                Debug.LogWarning(error.ErrorMessage);
            }
        }
    }
}