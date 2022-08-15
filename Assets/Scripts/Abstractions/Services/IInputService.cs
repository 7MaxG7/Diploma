﻿using UnityEngine;


namespace Services {

	internal interface IInputService {
		Vector2 Axis { get; }
		bool CompassButtonIsPressed { get; }
		
		void Init();
	}

}