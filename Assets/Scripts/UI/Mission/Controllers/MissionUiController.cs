using System;
using Infrastructure;
using Units;
using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


namespace UI {

	internal class MissionUiController : IMissionUiController {
		public event Action<WeaponType> OnSkillChoose;
		private event Action OnUpdateCallback;

		private readonly IPermanentUiController _permanentUiController;
		private readonly UiConfig _uiConfig;
		private PlayerUiController _playerUiController;
		private SkillsUiController _skillsUiController;
		private MissionUiView _missionUiView;
		private bool _isInited;


		[Inject]
		public MissionUiController(UiConfig uiConfig, IControllersHolder controllersHolder, IPermanentUiController permanentUiController) {
			controllersHolder.AddController(this);
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
			_missionUiView.SettingsButton.onClick.RemoveAllListeners();
			Object.Destroy(_missionUiView.gameObject);
		}

		public void OnUpdate(float deltaTime) {
			if (!_isInited)
				return;

			OnUpdateCallback?.Invoke();
		}
		
		public void Init(IUnit player) {
			var uiRoot = GameObject.Find(TextConstants.UI_ROOT_NAME) ?? new GameObject(TextConstants.UI_ROOT_NAME);
			_missionUiView = Object.Instantiate(_uiConfig.MissionUiView, uiRoot.transform);
			_playerUiController = new PlayerUiController(_missionUiView.PlayerPanel, _uiConfig);
			_playerUiController.Init(player);
			OnUpdateCallback += _playerUiController.UpdateSmoothers;
			
			_missionUiView.SkillsPanel.CanvasGroup.alpha = 0;
			_skillsUiController = new SkillsUiController(_missionUiView.SkillsPanel, _uiConfig);
			_skillsUiController.OnSkillChoose += UpgradeSkill;
			
			_missionUiView.SettingsButton.onClick.AddListener(ShowSettings);
			
			_missionUiView.CompassPointerCanvasGroup.alpha = 0;
			_missionUiView.CompassPointerCanvasGroup.gameObject.SetActive(false);
				
			_isInited = true;
		}

		public void ShowSkillsChoose(ActualSkillInfo[] skills) {
			_skillsUiController.ShowSkillsChoose(skills);
		}

		public void ShowCompass(Vector3 closestEnemyPlayerDestination) {
			if (!_isInited)
				return;
			
			_missionUiView.CompassPointerCanvasGroup.gameObject.SetActive(true);
			if (_missionUiView.CompassPointerCanvasGroup.alpha < 1)
				_missionUiView.CompassPointerCanvasGroup.alpha += _uiConfig.ArrowPointerFadingFrameDelta;
			_missionUiView.CompassPointerCanvasGroup.transform.up = closestEnemyPlayerDestination;
		}

		public void HideCompass(Vector3 closestEnemyPlayerDestination) {
			if (!_isInited)
				return;
			
			if (!_missionUiView.CompassPointerCanvasGroup.gameObject.activeSelf)
				return;
			
			if (_missionUiView.CompassPointerCanvasGroup.alpha > 0) {
				_missionUiView.CompassPointerCanvasGroup.alpha -= _uiConfig.ArrowPointerFadingFrameDelta;
				_missionUiView.CompassPointerCanvasGroup.transform.up = closestEnemyPlayerDestination;
			}
			else
				_missionUiView.CompassPointerCanvasGroup.gameObject.SetActive(false);
		}

		private void UpgradeSkill(WeaponType weaponType) {
			OnSkillChoose?.Invoke(weaponType);
		}

		private void ShowSettings() {
			_permanentUiController.ShowSettingsPanel(missionPanelIsActive: true);
		}
	}

}