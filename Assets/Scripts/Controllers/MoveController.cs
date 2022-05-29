using Infrastructure;
using Services;
using UnityEngine;
using Utils;


namespace Controllers {

	internal sealed class MoveController : IUpdater {
		private readonly IInputService _inputService;
		private readonly CharacterController _characterController;
		private float _moveSpeed = 5;
		private Vector2 _moveDiredtion;
		private Camera _camera;


		public MoveController(CharacterController characterController, IInputService inputService) {
			_characterController = characterController;
			_inputService = inputService;
		}

		public void Init() {
			_camera = Camera.main;
		}

		public void OnUpdate(float deltaTime) {
			if (_inputService.Axis.sqrMagnitude < Constants.CHARACTER_SPEED_STOP_TRESHOLD)
				return;
			
			var moveDiredtion = _camera.transform.TransformDirection(_inputService.Axis);
			moveDiredtion.Normalize();
			_characterController.transform.up = moveDiredtion;
			_characterController.Move(moveDiredtion * deltaTime * _moveSpeed);
		}
	}

}