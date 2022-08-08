using System;
using System.Collections.Generic;
using Infrastructure;


namespace Units {

	internal class Health {
		public event Action<int> OnMaxHpChange;
		public event Action<int> OnCurrentHpChange;
		public event Action<DamageInfo> OnDied;

		public int CurrentHp {
			get => _currentHp;
			private set {
				if (_currentHp != value) {
					_currentHp = value;
					OnCurrentHpChange?.Invoke(value);
				}
			}
		}
		public int MaxHp {
			get => _maxHp;
			private set {
				if (_maxHp != value) {
					_maxHp = value;
					OnMaxHpChange?.Invoke(value);
				}
			}
		}
		private int BaseHp { get; set; }

		private int _currentHp;
		private int _maxHp;
		private Dictionary<int, int> _additionalLevelHps;


		public Health(int hp) {
			BaseHp = hp;
			MaxHp = hp;
			CurrentHp = hp;
		}

		public void TakeDamage(DamageInfo damageInfo) {
			CurrentHp -= Math.Min(damageInfo.Damage, CurrentHp);
			if (CurrentHp <= 0)
				OnDied?.Invoke(damageInfo);
		}

		public void Kill(IUnit damageTaker) {
			TakeDamage(new DamageInfo(CurrentHp, null, damageTaker));
		}

		public void Restore() {
			CurrentHp = BaseHp;
		}

		public void AddLevelUpHealth(int level) {
			CurrentHp += _additionalLevelHps[level];
			BaseHp += _additionalLevelHps[level];
			MaxHp += _additionalLevelHps[level];
		}

		public void SetLevelUpHpParams(PlayerConfig.LevelHealthParam[] levelHpParameters) {
			_additionalLevelHps = new Dictionary<int, int>();
			for (var i = 1; i < levelHpParameters.Length; i++) {
				_additionalLevelHps.Add(levelHpParameters[i].Level, levelHpParameters[i].Health - levelHpParameters[i - 1].Health);
			}
		}
	}

}