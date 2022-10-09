using UnityEngine;


namespace Units {

	internal sealed class PlayerView : UnitView {
		
		public override void Move(Vector3 deltaPosition) {
			base.Move(deltaPosition);
			_transform.up = deltaPosition;
		}
	}

}