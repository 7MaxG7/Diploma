using System;
using Units.Views;


namespace Units {

	internal class PlayerView : UnitView, IDamagableView {
		public event Action<int> OnDamageTake;
		
		public void TakeDamage(int damage) {
			OnDamageTake?.Invoke(damage);
		}
	}

}