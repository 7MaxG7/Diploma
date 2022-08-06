using System;
using Infrastructure;
using Units;
using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


namespace UI {

	internal class MissionUiController : IMissionUiController, IDisposable {
		public event Action<WeaponType> OnSkillChoose;
		private event Action OnUpdateCallback;

		private readonly UiConfig _uiConfig;
		private PlayerUiController _playerUiController;
		private SkillsUiController _skillsUiController;
		private MissionUiView _missionUiView;
		private readonly IPermanentUiController _permanentUiController;
		private bool _isInited;


		[Inject]
		public MissionUiController(UiConfig uiConfig, IControllersHolder controllersHolder, IPermanentUiController permanentUiController) {
			controllersHolder.AddController(this);
			_uiConfig = uiConfig;
			_permanentUiController = permanentUiController;
		}

		public void Dispose() {
			OnUpdateCallback -= _playerUiController.UpdateSmoothers;
			_skillsUiController.OnSkillChoose -= UpgradeSkill;
			_playerUiController?.Dispose();
			_missionUiView.SettingsButton.onClick.RemoveAllListeners();
		}

		public void OnUpdate(float deltaTime) {
			if (!_isInited)
				return;

			OnUpdateCallback?.Invoke();
		}

		public void Init(IUnit player) {
			_missionUiView = Object.Instantiate(_uiConfig.MissionUiView, new GameObject(TextConstants.UI_ROOT_NAME).transform);
			_playerUiController = new PlayerUiController(_missionUiView.PlayerPanel, _uiConfig);
			_playerUiController.Init(player);
			OnUpdateCallback += _playerUiController.UpdateSmoothers;
			
			_missionUiView.SkillsPanel.CanvasGroup.alpha = 0;
			_skillsUiController = new SkillsUiController(_missionUiView.SkillsPanel, _uiConfig);
			_skillsUiController.OnSkillChoose += UpgradeSkill;
			
			_missionUiView.SettingsButton.onClick.AddListener(ShowSettings);
			
			_isInited = true;
		}

		public void ShowSkillsChoose(ActualSkillInfo[] skills) {
			_skillsUiController.ShowSkillsChoose(skills);
		}

		private void UpgradeSkill(WeaponType weaponType) {
			OnSkillChoose?.Invoke(weaponType);
		}

		private void ShowSettings() {
			_permanentUiController.ShowSettingsPanel(missionPanelIsActive: true);
		}
	}

}