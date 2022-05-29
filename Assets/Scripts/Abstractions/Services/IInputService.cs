using UnityEngine;


namespace Services {

	internal interface IInputService {
		Vector2 Axis { get; }
		void Init();
	}

}