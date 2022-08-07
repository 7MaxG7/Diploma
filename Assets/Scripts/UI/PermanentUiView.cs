using TMPro;
using UnityEngine;


namespace UI {

	internal sealed class PermanentUiView : MonoBehaviour, IPermanentUiView {
		[SerializeField] private Canvas _permanentCanvas;
		[SerializeField] private CanvasGroup _loadingCurtainCanvasGroup;
		[SerializeField] private TMP_Text _loadingCurtainText;
		[SerializeField] private SettingsPanelView _settingsPanel;
		[SerializeField] private GameObject _missionSettingsPanel;
		[SerializeField] private ResultPanelView _resultPanel;

		public GameObject GameObject => gameObject;
		public Canvas PermanentCanvas => _permanentCanvas;
		public CanvasGroup LoadingCurtainCanvasGroup => _loadingCurtainCanvasGroup;
		public TMP_Text LoadingCurtainText => _loadingCurtainText;
		public SettingsPanelView SettingsPanel => _settingsPanel;
		public GameObject MissionSettingsPanel => _missionSettingsPanel;
		public ResultPanelView ResultPanel => _resultPanel;
	}

}