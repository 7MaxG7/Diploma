using UnityEngine;


namespace Infrastructure {

	internal class CameraController : ILateUpdater {
		private Transform _target;
		private Camera _mainCamera;
		private Vector3 _cameraOffset;


		public void OnLateUpdate(float deltaTime) {
			if (_target == null)
				return;

			_mainCamera.transform.position = _target.transform.position + _cameraOffset;
		}

		public void Init() {
			_mainCamera = Camera.main;
		}

		public void Follow(Transform target, Vector3 cameraOffset) {
			_target = target;
			_cameraOffset = cameraOffset;
		}

	}

}