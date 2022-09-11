using System;
using UnityEngine;


namespace Infrastructure {

	internal interface ICameraManager : ILateUpdater, IDisposable {
		bool CameraIsPositioned { get; }
		
		void Follow(Transform target, Vector3 cameraOffset);
	}

}