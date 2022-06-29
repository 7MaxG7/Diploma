using System;
using Infrastructure;
using Services;
using Units;
using UnityEngine;
using Utils;
using Zenject;


namespace Controllers {

	internal sealed class PlayerMoveController : IPlayerMoveController, IDisposable {
		private readonly IMapWrapper _mapWrapper;
		private readonly IInputService _inputService;
		private IUnit _player;
		private Vector2 _moveDiredtion;
		private Camera _camera;


		[Inject]
		public PlayerMoveController(IMapWrapper mapWrapper, IInputService inputService, IControllersHolder controllersHolder) {
			_mapWrapper = mapWrapper;
			_inputService = inputService;
			controllersHolder.AddController(this);
		}

		public void Dispose() {
			OnDispose();
		}

		public void Init(IUnit player) {
			_camera = Camera.main;
			_player = player;
		}

		public void OnUpdate(float deltaTime) {
			if (_player == null || _camera == null)
				return;
			
			if (_inputService.Axis.sqrMagnitude < Constants.CHARACTER_SPEED_STOP_TRESHOLD)
				return;
			
			var moveDiredtion = _camera.transform.TransformDirection(_inputService.Axis);
			moveDiredtion.Normalize();
			_player.Transform.up = moveDiredtion;
			_player.CharacterController.Move(moveDiredtion * (deltaTime * _player.MoveSpeed));
			_mapWrapper.CheckAndReturnInsideMap(_player.Transform);
		}

		private void OnDispose() {
			_player = null;
		}

	}

}