using System.Linq;
using Infrastructure;
using Services;
using Units;
using UnityEngine;
using Utils;
using Zenject;


namespace Controllers {

	internal sealed class PlayerMoveController : IPlayerMoveController {
		public IUnit Player { get; private set; }

		private readonly IInputService _inputService;
		private IMapWrapper _mapWrapper;
		private IUnitsPool _unitsPool;
		private IMissionMapController _missionMapController;
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
			_camera = null;
			_mapWrapper = null;
			_unitsPool = null;
			_missionMapController = null;
			Player = null;
		}

		public void Init(IUnit player) {
			_camera = Camera.main;
			Player = player;
		}

		public void OnFixedUpdate(float deltaTime) {
			if (Player == null || _camera == null || Player.IsDead)
				return;
			
			if (_inputService.Axis.sqrMagnitude < Constants.CHARACTER_SPEED_STOP_TRESHOLD)
				return;
			
			var moveDiredtion = _camera.transform.TransformDirection(_inputService.Axis);
			moveDiredtion.Normalize();
			Player.Transform.up = moveDiredtion;
			Player.Rigidbody.MovePosition(Player.Transform.position + moveDiredtion * (deltaTime * Player.MoveSpeed));
			_mapWrapper.CheckAndReturnInsideMap(Player.Transform, _unitsPool.ActiveMonsters.Select(monster => monster.Transform).Concat(_missionMapController.GroundItems));
		}
	}

}