using UnityEngine;
using Utils;


namespace Services {

	internal class MobileInputService : InputService {
		public override Vector2 Axis => 
				new(SimpleInput.GetAxis(Constants.HORIZONTAL), SimpleInput.GetAxis(Constants.VERTICAL));
		// TODO. Implement mobile button
		public override bool CompassButtonIsPressed { get; }


		public override void Init() {
		}
	}

}