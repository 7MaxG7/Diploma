using UnityEngine;
using Utils;


namespace Services {

	class MobileInputService : InputService {
		public override Vector2 Axis => 
				new(SimpleInput.GetAxis(TextConstants.HORIZONTAL), SimpleInput.GetAxis(TextConstants.VERTICAL));
		// TODO. Implement mobile button
		public override bool CompassButtonIsPressed { get; }


		public override void Init() {
		}
	}

}