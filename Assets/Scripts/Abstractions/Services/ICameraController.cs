using System;
using UnityEngine;


namespace Infrastructure {

	internal interface ICameraController : ILateUpdater, IDisposable {
		bool CameraIsPositioned { get; }
		
		void Follow(Transform target, Vector3 cameraOffset);
	}

}