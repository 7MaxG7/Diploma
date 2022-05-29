using UnityEngine;


namespace Services {

	public class PcInputService : InputService {
		public override Vector2 Axis => _axis;

		private readonly InputControls _inputControls = new ();
		private Vector2 _axis;


		public override void Init() {
			_inputControls.Enable();
			
			_inputControls.Mission.Move.performed += context => 
					_axis = context.ReadValue<Vector2>();
			_inputControls.Mission.Move.canceled += _ =>
					_axis = Vector2.zero;
		}
	}

}