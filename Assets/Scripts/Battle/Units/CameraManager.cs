using UnityEngine;
using Zenject;


namespace Infrastructure {

	internal sealed class CameraManager : ICameraManager {
		public bool CameraIsPositioned { get; private set; }
		
		private Transform _target;
		private Camera _mainCamera;
		private Vector3 _cameraOffset;


		[Inject]
		public CameraManager(IControllersHolder controllersHolder) {
			controllersHolder.AddController(this);
		}

		public void Dispose() {
			_target = null;
			_mainCamera = null;
		}

		public void OnLateUpdate(float deltaTime) {
			if (_target == null || _mainCamera == null) {
				CameraIsPositioned = false;
				return;
			}

			_mainCamera.transform.position = _target.transform.position + _cameraOffset;
			CameraIsPositioned = true;
		}

		public void Follow(Transform target, Vector3 cameraOffset) {
			_target = target;
			_cameraOffset = cameraOffset;
			_mainCamera = Camera.main;
		}

	}

}