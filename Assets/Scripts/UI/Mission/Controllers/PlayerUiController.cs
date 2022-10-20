using System;
using Infrastructure;
using Units;
using Utils;


namespace UI
{
    internal sealed class PlayerUiController : IDisposable
    {
        private IUnit _player;
        private readonly PlayerPanelView _playerPanel;
        private readonly UiConfig _uiConfig;
        private Smoother _currentHealthSmoother;
        private Smoother _maxHealthSmoother;
        private Smoother _expSmoother;

        private float _currentHp;
        private float _maxHp;
        private int _currentLevel;
        private int _currentLevelMinExp;
        private int _currentLevelMaxExp;


        public PlayerUiController(PlayerPanelView playerPanel, UiConfig uiConfig)
        {
            _playerPanel = playerPanel;
            _uiConfig = uiConfig;
            _currentHealthSmoother = new Smoother(uiConfig.HpBarAnimationDurationInFrames);
            _maxHealthSmoother = new Smoother(uiConfig.HpBarAnimationDurationInFrames);
            _expSmoother = new Smoother(uiConfig.HpBarAnimationDurationInFrames);
        }

        public void Dispose()
        {
            _currentHealthSmoother.OnValueUpdateCallback -= UpdateCurrentHealth;
            _player.Health.OnCurrentHpChange -= UpdateCurrentHealthSmoothly;
            _maxHealthSmoother.OnValueUpdateCallback -= UpdateMaxHealth;
            _player.Health.OnMaxHpChange -= UpdateMaxHealthSmoothly;
            _expSmoother.OnValueUpdateCallback -= UpdateCurrentExperience;
            _player.Experience.OnExpChange -= UpdateCurrentExperienceSmoothly;
            _currentHealthSmoother = null;
            _maxHealthSmoother = null;
            _expSmoother = null;
            _player = null;
        }

        public void Init(IUnit player)
        {
            _player = player;
            _playerPanel.Init(_uiConfig);

            UpdateCurrentHealth(_player.Health.CurrentHp);
            _currentHealthSmoother.OnValueUpdateCallback += UpdateCurrentHealth;
            _player.Health.OnCurrentHpChange += UpdateCurrentHealthSmoothly;
            _currentHealthSmoother.SetStartValue(_player.Health.CurrentHp);

            UpdateMaxHealth(_player.Health.MaxHp);
            _maxHealthSmoother.OnValueUpdateCallback += UpdateMaxHealth;
            _player.Health.OnMaxHpChange += UpdateMaxHealthSmoothly;
            _maxHealthSmoother.SetStartValue(_player.Health.MaxHp);

            _currentLevel = _player.Experience.CurrentLevel;
            _playerPanel.UpdateLevel(_currentLevel);
            _currentLevelMaxExp = _player.Experience.GetExpTarget();
            UpdateCurrentExperience(_player.Experience.CurrentExp);
            _expSmoother.OnValueUpdateCallback += UpdateCurrentExperience;
            _player.Experience.OnExpChange += UpdateCurrentExperienceSmoothly;
            _expSmoother.SetStartValue(_player.Experience.CurrentExp);
        }

        public void UpdateSmoothers()
        {
            _currentHealthSmoother.SmoothUpdate();
            _maxHealthSmoother.SmoothUpdate();
            _expSmoother.SmoothUpdate();
        }

        private void UpdateCurrentHealthSmoothly(int hp)
        {
            _currentHealthSmoother.SetTargetValue(hp);
        }

        private void UpdateMaxHealthSmoothly(int maxHp)
        {
            _maxHealthSmoother.SetTargetValue(maxHp);
        }

        private void UpdateCurrentHealth(float hp)
        {
            _currentHp = hp;
            _playerPanel.UpdateHealth(_currentHp, _maxHp);
        }

        private void UpdateMaxHealth(float maxHp)
        {
            _maxHp = maxHp;
            _playerPanel.UpdateHealth(_currentHp, _maxHp);
        }

        private void UpdateCurrentExperienceSmoothly(int xp)
        {
            _expSmoother.SetTargetValue(xp);
        }

        private void UpdateCurrentExperience(float xp)
        {
            if (xp >= _currentLevelMaxExp)
            {
                _currentLevelMinExp = _currentLevelMaxExp;
                _playerPanel.UpdateLevel(++_currentLevel);
                _currentLevelMaxExp = _player.Experience.GetExpTarget(_currentLevel + 1);
            }

            if (xp < _player.Experience.MaxExp)
                _playerPanel.UpdateExperience((int)xp, _currentLevelMinExp, _currentLevelMaxExp);
            else
                _playerPanel.UpdateExperience(_currentLevelMaxExp, _currentLevelMinExp, _currentLevelMaxExp);
        }
    }
}