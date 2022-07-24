using System;
using System.Linq;
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
		private readonly IUnitsPool _unitsPool;
		private readonly IMissionMapController _missionMapController;
		private IUnit _player;
		private Vector2 _moveDiredtion;
		private Camera _camera;


		[Inject]
		public PlayerMoveController(IMapWrapper mapWrapper, IInputService inputService, IUnitsPool unitsPool, IMissionMapController missionMapController
				, IControllersHolder controllersHolder) {
			_mapWrapper = mapWrapper;
			_inputService = inputService;
			_unitsPool = unitsPool;
			_missionMapController = missionMapController;
			controllersHolder.AddController(this);
		}

		public void Dispose() {
			OnDispose();
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
			
			var moveDiredtion = _camera.transform.TransformDirection(_inputService.Axis);
			moveDiredtion.Normalize();
			_player.Transform.up = moveDiredtion;
			_player.Rigidbody.MovePosition(_player.Transform.position + moveDiredtion * (deltaTime * _player.MoveSpeed));
			_mapWrapper.CheckAndReturnInsideMap(_player.Transform, _unitsPool.ActiveMonsters.Select(monster => monster.Transform).Concat(_missionMapController.GroundItems));
		}

		private void OnDispose() {
			_player = null;
		}

		public void OnFixedUpdate() {
			throw new NotImplementedException();
		}
	}

}