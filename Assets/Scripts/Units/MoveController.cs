using Infrastructure;
using Services;
using UnityEngine;
using Utils;
using Zenject;


namespace Controllers {

	internal sealed class MoveController : IMoveController,IDisposer {
		private readonly IInputService _inputService;
		private CharacterController _characterController;
		private float _moveSpeed = 5;
		private Vector2 _moveDiredtion;
		private Camera _camera;
		private bool _isInited;


		[Inject]
		public MoveController(IInputService inputService, IControllersHolder controllersHolder) {
			_inputService = inputService;
			controllersHolder.AddController(this);
		}

		public void Init(CharacterController characterController) {
			_isInited = true;
			_camera = Camera.main;
			_characterController = characterController;
		}

		public void OnUpdate(float deltaTime) {
			if (!_isInited)
				return;
			
			if (_inputService.Axis.sqrMagnitude < Constants.CHARACTER_SPEED_STOP_TRESHOLD)
				return;
			
			var moveDiredtion = _camera.transform.TransformDirection(_inputService.Axis);
			moveDiredtion.Normalize();
			_characterController.transform.up = moveDiredtion;
			_characterController.Move(moveDiredtion * deltaTime * _moveSpeed);
		}

		public void OnDispose() {
			_isInited = false;
		}
	}

}