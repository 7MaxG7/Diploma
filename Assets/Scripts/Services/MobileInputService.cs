using UnityEngine;
using Utils;


namespace Services {

	class MobileInputService : InputService {
		public override Vector2 Axis => 
				new(SimpleInput.GetAxis(TextConstants.HORIZONTAL), SimpleInput.GetAxis(TextConstants.VERTICAL));

		
		public override void Init() {
		}
	}

}