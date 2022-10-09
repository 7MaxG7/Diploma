using Infrastructure;
using Services;
using Units;
using UnityEngine;
using Utils;
using Zenject;


namespace Controllers {

	internal sealed class PlayerMoveManager : IPlayerMoveManager {
		private readonly IInputService _inputService;
		private IUnit _player;
		private Vector2 _moveDiredtion;
		private Camera _camera;


		[Inject]
		public PlayerMoveManager(IInputService inputService, IControllersHolder controllersHolder) {
			_inputService = inputService;
			controllersHolder.AddController(this);
		}

		public void Dispose() {
			_camera = null;
			_player = null;
		}

		public void Init(IUnit player) {
			_camera = Camera.main;
			_player = player;
		}

		public void OnFixedUpdate(float deltaTime) {
			if (_player == null || _camera == null || _player.IsDead)
				return;
			
			if (_inputService.Axis.sqrMagnitude < Constants.CHARACTER_SPEED_STOP_TRESHOLD)
				return;
			
			var moveDiredtion = _camera.transform.TransformDirection(_inputService.Axis).normalized;
			_player.Move(moveDiredtion * (deltaTime * _player.MoveSpeed));
		}
	}

}