using System;
using Infrastructure;
using Services;
using Units;
using UnityEngine;
using Weapons;
using Zenject;
using Object = UnityEngine.Object;


namespace UI {

	internal sealed class MissionUiController : IMissionUiController {
		public event Action<WeaponType> OnSkillChoose;
		private event Action OnUpdateCallback;

		private readonly IPermanentUiController _permanentUiController;
		private readonly IViewsFactory _viewsFactory;
		private readonly UiConfig _uiConfig;
		private PlayerUiController _playerUiController;
		private SkillsUiController _skillsUiController;
		private MissionUiView _missionUiView;
		private bool _isInited;


		[Inject]
		public MissionUiController(IViewsFactory viewsFactory, UiConfig uiConfig, IControllersHolder controllersHolder, IPermanentUiController permanentUiController) {
			controllersHolder.AddController(this);
			_viewsFactory = viewsFactory;
			_uiConfig = uiConfig;
			_permanentUiController = permanentUiController;
		}

		public void Dispose() {
			_isInited = false;
			OnUpdateCallback -= _playerUiController.UpdateSmoothers;
			_skillsUiController.OnSkillChoose -= UpgradeSkill;
			_skillsUiController.Dispose();
			_skillsUiController = null;
			_playerUiController?.Dispose();
			_playerUiController = null;
			_missionUiView.OnSettingsClick -= ShowSettings;
			_missionUiView.OnDispose();
			Object.Destroy(_missionUiView.gameObject);
		}

		public void OnUpdate(float deltaTime) {
			if (!_isInited)
				return;

			OnUpdateCallback?.Invoke();
		}
		
		public void Init(IUnit player) {
			_missionUiView = _viewsFactory.CreateMissionUi();
			_playerUiController = new PlayerUiController(_missionUiView.PlayerPanel, _uiConfig);
			_playerUiController.Init(player);
			OnUpdateCallback += _playerUiController.UpdateSmoothers;
			
			_skillsUiController = new SkillsUiController(_missionUiView.SkillsPanel, _uiConfig);
			_skillsUiController.Init();
			_skillsUiController.OnSkillChoose += UpgradeSkill;
			
			_missionUiView.Init(_uiConfig);
			_missionUiView.OnSettingsClick += ShowSettings;
			
			_isInited = true;
		}

		public void ShowSkillsChoose(ActualSkillInfo[] skills) {
			_skillsUiController.ShowSkillsChoose(skills);
		}

		public void ShowCompass(Vector3 closestEnemyPlayerDestination) {
			if (!_isInited)
				return;

			_missionUiView.ShowCompass(closestEnemyPlayerDestination);
		}

		public void HideCompass(Vector3 closestEnemyPlayerDestination) {
			if (!_isInited)
				return;

			_missionUiView.HideCompass(closestEnemyPlayerDestination);
		}

		private void UpgradeSkill(WeaponType weaponType) {
			OnSkillChoose?.Invoke(weaponType);
		}

		private void ShowSettings() {
			_permanentUiController.ShowSettingsPanel(missionSettingsSectionIsActive: true);
		}
	}

}