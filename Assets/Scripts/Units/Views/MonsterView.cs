using System;
using UnityEngine;


namespace Units.Views {

	internal sealed class MonsterView : UnitView {
		public event Action<ControllerColliderHit> OnCollisionEnter; 

		
		private void OnControllerColliderHit(ControllerColliderHit hit) {
			OnCollisionEnter?.Invoke(hit);
		}
	}

}