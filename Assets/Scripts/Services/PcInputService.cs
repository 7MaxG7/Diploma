using UnityEngine;


namespace Services {

	internal sealed class PcInputService : InputService {
		public override Vector2 Axis => _axis;
		public override bool CompassButtonIsPressed => _compassButtonIsPressed;

		private readonly InputControls _inputControls = new ();
		private Vector2 _axis;
		private bool _compassButtonIsPressed;


		public override void Init() {
			_inputControls.Enable();
			
			_inputControls.Mission.Move.performed += context => _axis = context.ReadValue<Vector2>();
			_inputControls.Mission.Move.canceled += _ => _axis = Vector2.zero;
			_inputControls.Mission.CompassButton.performed += _ => _compassButtonIsPressed = true;
			_inputControls.Mission.CompassButton.canceled += _ => _compassButtonIsPressed = false;
		}
	}

}