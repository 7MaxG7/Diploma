using System;
using Infrastructure;
using Units;
using Utils;


namespace UI {

	internal class PlayerUiController : IDisposable {
		private IUnit _player;
		private readonly PlayerPanelView _playerPanel;
		private readonly Smoother _healthSmoother;
		private readonly Smoother _expSmoother;
		
		private int _maxHp;
		private int _currentLevel;
		private int _currentLevelMaxExp;


		public PlayerUiController(PlayerPanelView playerPanel, UiConfig uiConfig) {
			_playerPanel = playerPanel;
			_healthSmoother = new Smoother(uiConfig.HpBarAnimationDurationInFrames);
			_expSmoother = new Smoother(uiConfig.HpBarAnimationDurationInFrames);
		}

		public void Dispose() {
			_player.Health.OnCurrentHpChange -= UpdateCurrentHealthSmoothly;
			_healthSmoother.OnValueUpdateCallback -= UpdateCurrentHealth;
		}

		public void Init(IUnit player) {
			_player = player;
			_maxHp = _player.Health.MaxHp;
			UpdateCurrentHealth(_player.Health.CurrentHp);
			_healthSmoother.OnValueUpdateCallback += UpdateCurrentHealth;
			_healthSmoother.SetStartValue(_player.Health.CurrentHp);
			_player.Health.OnCurrentHpChange += UpdateCurrentHealthSmoothly;

			_currentLevel = _player.Experience.CurrentLevel;
			_playerPanel.LevelText.text = _player.Experience.CurrentLevel.ToString();
			_currentLevelMaxExp = _player.Experience.GetExpTarget();
			UpdateCurrentExperience(_player.Experience.CurrentExp);
			_expSmoother.OnValueUpdateCallback += UpdateCurrentExperience;
			_expSmoother.SetStartValue(_player.Experience.CurrentExp);
			_player.Experience.OnExpChange += UpdateCurrentExperienceSmoothly;
		}

		public void UpdateSmoother() {
			_healthSmoother.SmoothUpdate();
			_expSmoother.SmoothUpdate();
		}

		private void UpdateCurrentHealthSmoothly(int hp) {
			_healthSmoother.SetTargetValue(hp);
		}

		private void UpdateCurrentHealth(float hp) {
			_playerPanel.HealthSlider.value = hp / _maxHp;
			_playerPanel.HealthText.text = string.Format(TextConstants.HEALTH_BAR_TEXT_TEMPLATE, (int)hp, _maxHp);
		}
		
		private void UpdateCurrentExperienceSmoothly(int xp) {
			_expSmoother.SetTargetValue(xp);
		}

		private void UpdateCurrentExperience(float xp) {
			var currentExp = xp;
			if (xp >= _currentLevelMaxExp) {
				_playerPanel.LevelText.text = $"{++_currentLevel}";
				currentExp -= _currentLevelMaxExp;
				_currentLevelMaxExp = _player.Experience.GetExpTarget(_currentLevel + 1);
			}
			_playerPanel.ExpSlider.value = currentExp / _currentLevelMaxExp;
		}
	}

}