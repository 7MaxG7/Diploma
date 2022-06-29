using UnityEngine;


namespace Infrastructure {

	internal interface ICameraController : ILateUpdater {
		void Follow(Transform target, Vector3 cameraOffset);

		bool CameraIsPositioned { get; }
	}

}