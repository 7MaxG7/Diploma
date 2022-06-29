using System;


namespace Units {

	internal class Health {
		public event Action<int> OnCurrentHpChange;
		public event Action OnDied;

		public int CurrentHp {
			get => _currentHp;
			private set {
				if (_currentHp != value) {
					OnCurrentHpChange?.Invoke(value);
					_currentHp = value;
				}
			}
		}
		private int BaseHp { get; }
		private int MaxHp { get; }
		
		private int _currentHp;


		public Health(int hp) {
			BaseHp = hp;
			MaxHp = hp;
			CurrentHp = hp;
		}

		public void TakeDamage(int damage) {
			CurrentHp -= Math.Min(damage, CurrentHp);
			if (CurrentHp <= 0)
				OnDied?.Invoke();
		}

		public void Kill() {
			TakeDamage(CurrentHp);
		}
	}

}