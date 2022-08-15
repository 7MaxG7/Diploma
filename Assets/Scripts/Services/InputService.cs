using UnityEngine;


namespace Services {

	public abstract class InputService : IInputService {
		public abstract Vector2 Axis { get; }
		public abstract bool CompassButtonIsPressed { get; }

		
		public abstract void Init();
	}

}