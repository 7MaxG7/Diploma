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
		private bool _isInited;


		[Inject]
		public MissionUiController(UiConfig uiConfig, IControllersHolder controllersHolder) {
			controllersHolder.AddController(this);
			_uiConfig = uiConfig;
		}

		public void Dispose() {
			OnUpdateCallback -= _playerUiController.UpdateSmoothers;
			_skillsUiController.OnSkillChoose -= UpgradeSkill;
			_playerUiController?.Dispose();
		}

		public void OnUpdate(float deltaTime) {
			if (!_isInited)
				return;

			OnUpdateCallback?.Invoke();
		}

		public void Init(IUnit player) {
			var missionUiView = Object.Instantiate(_uiConfig.MissionUiView, new GameObject(TextConstants.UI_ROOT_NAME).transform);
			_playerUiController = new PlayerUiController(missionUiView.PlayerPanel, _uiConfig);
			_playerUiController.Init(player);
			OnUpdateCallback += _playerUiController.UpdateSmoothers;
			
			missionUiView.SkillsPanel.CanvasGroup.alpha = 0;
			_skillsUiController = new SkillsUiController(missionUiView.SkillsPanel, _uiConfig);
			_skillsUiController.OnSkillChoose += UpgradeSkill;
			_isInited = true;
		}

		private void UpgradeSkill(WeaponType weaponType) {
			OnSkillChoose?.Invoke(weaponType);
		}

		public void ShowSkillsChoose(ActualSkillInfo[] skills) {
			_skillsUiController.ShowSkillsChoose(skills);
		}
	}

}