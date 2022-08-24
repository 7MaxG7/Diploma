using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure {

	internal class MainMenuView : MonoBehaviour, IMainMenuView {
		[SerializeField] private GameObject _gameObject;
		[SerializeField] private TMP_Text _headerLabel;
		[SerializeField] private TMP_Text _scoreLable;
		[SerializeField] private TMP_Text _loginButtonText;
		[SerializeField] private Button _loginPanelButton;
		[SerializeField] private Button _playButton;
		[SerializeField] private Button _settingsButton;
		[SerializeField] private Button _howToPlayButton;
		[SerializeField] private Button _creditsButton;
		[SerializeField] private Button _quitGameButton;
		[Header("Panels")]
		[SerializeField] private LoginPanelView _loginPanelView;
		[SerializeField] private LobbyScreenView _lobbyScreenView;
		[SerializeField] private CreditsView _creditsView;
		[SerializeField] private HowToPlayView _howToPlayView;

		public GameObject GameObject => _gameObject;
		public TMP_Text HeaderLabel => _headerLabel;
		public TMP_Text LoginButtonText => _loginButtonText;
		public Button LoginPanelButton => _loginPanelButton;
		public Button PlayButton => _playButton;
		public Button SettingsButton => _settingsButton;
		public Button HowToPlayButton => _howToPlayButton;
		public Button CreditsButton => _creditsButton;
		public Button QuitGameButton => _quitGameButton;
		// Panels
		public LoginPanelView LoginPanelView => _loginPanelView;
		public LobbyScreenView LobbyScreenView => _lobbyScreenView;
		public CreditsView CreditsView => _creditsView;
		public TMP_Text ScoreLable => _scoreLable;
		public HowToPlayView HowToPlayView => _howToPlayView;
	}

}