using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Infrastructure.Zenject {

	internal class MainMenuView : MonoBehaviour, IMainMenuView {
		[SerializeField] private GameObject _gameObject;
		[SerializeField] private TMP_Text _headerLabel;
		[SerializeField] private TMP_Text _loginButtonText;
		[SerializeField] private Button _loginPanelButton;
		[SerializeField] private Button _playButton;
		[SerializeField] private Button _quitGameButton;
		[Header("Panels")]
		[SerializeField] private LoginPanelView _loginPanelView;
		[SerializeField] private LobbyScreenView _lobbyScreenView;

		public GameObject GameObject => _gameObject;
		public TMP_Text HeaderLabel => _headerLabel;
		public TMP_Text LoginButtonText => _loginButtonText;
		public Button LoginPanelButton => _loginPanelButton;
		public Button PlayButton => _playButton;
		public Button QuitGameButton => _quitGameButton;
		// Panels
		public LoginPanelView LoginPanelView => _loginPanelView;
		public LobbyScreenView LobbyScreenView => _lobbyScreenView;
	}

}