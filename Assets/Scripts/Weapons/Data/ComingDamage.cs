using System;
using Units;


namespace Infrastructure {

	internal class ComingDamage {
		public IDamagableView DamageTaker { get; }
		public int Damage { get; }
		public IUnit Damager { get; }
		public bool IsReady => _elapsedDelay <= 0;
		public bool IsCanceled { get; private set; }

		private float _elapsedDelay;

		public ComingDamage(float elapsedDelay, IDamagableView damageTaker, int damage, IUnit damager) {
			_elapsedDelay = elapsedDelay;
			DamageTaker = damageTaker;
			Damage = damage;
			Damager = damager;
		}

		public void ReduceDelay(float deltaTime) {
			_elapsedDelay -= Math.Min(deltaTime, _elapsedDelay);
		}

		public void StopDamage() {
			IsCanceled = true;
		}
	}

}