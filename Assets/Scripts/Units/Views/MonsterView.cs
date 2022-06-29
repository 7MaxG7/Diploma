using System;
using UnityEngine;


namespace Units.Views {

	internal sealed class MonsterView : UnitView, IDamagableView {
		public event Action<ControllerColliderHit> OnCollisionEnter; 
		public event Action<int> OnDamageTake;

		private void OnControllerColliderHit(ControllerColliderHit hit) {
			OnCollisionEnter?.Invoke(hit);
		}

		public void TakeDamage(int damage) {
			OnDamageTake?.Invoke(damage);
		}
	}

}