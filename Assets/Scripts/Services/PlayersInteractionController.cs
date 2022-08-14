using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Infrastructure;
using Units;
using UnityEngine;
using Zenject;


namespace Services {

	internal class PlayersInteractionController : IPlayersInteractionController {
		public bool IsPlayersFight => ClosestFightingEnemyPlayer != null;
		public PlayerView ClosestFightingEnemyPlayer { get; private set; } 
		public float ClosestFightingEnemyPlayerSqrMagnitude { get; private set; }

		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IMissionResultController _missionResultController;

		private List<PlayerView> EnemyPlayerViews { get; set; }
		private readonly float _maxPlayersFightSqrMagnitude;
		private IUnit _player;
		private bool _isInited;
		private bool _spawnerIsTurnedOffHere;


		[Inject]
		public PlayersInteractionController(IMonstersSpawner monstersSpawner, IMissionResultController missionResultController
				, MissionConfig missionConfig, IControllersHolder controllersHolder) {
			_monstersSpawner = monstersSpawner;
			_missionResultController = missionResultController;
			_maxPlayersFightSqrMagnitude = missionConfig.PlayersFightDistance * missionConfig.PlayersFightDistance;

			controllersHolder.AddController(this);
		}

		public void Dispose() {
			_isInited = false;
			_missionResultController.OnPlayerLeftRoomEvent -= RefindEnemyPlayerViews;
			_player = null;
			EnemyPlayerViews.Clear();
		}

		public void OnUpdate(float deltaTime) {
			if (!_isInited)
				return;
			
			ClosestFightingEnemyPlayer = null;
			ClosestFightingEnemyPlayerSqrMagnitude = float.MaxValue;
			foreach (var enemyPlayerView in EnemyPlayerViews.Where(view => view != null)) {
				var enemyPlayerSqrDistance = Vector3.SqrMagnitude(enemyPlayerView.Transform.position - _player.Transform.position);
				if (enemyPlayerSqrDistance <= _maxPlayersFightSqrMagnitude && enemyPlayerSqrDistance < ClosestFightingEnemyPlayerSqrMagnitude) {
					ClosestFightingEnemyPlayer = enemyPlayerView;
					ClosestFightingEnemyPlayerSqrMagnitude = enemyPlayerSqrDistance;
				}
			}
			
			if (!IsPlayersFight && _spawnerIsTurnedOffHere) {
				_monstersSpawner.StartSpawn();
				_spawnerIsTurnedOffHere = false;
			}
			if (IsPlayersFight && _monstersSpawner.SpawnIsOn) {
				_monstersSpawner.KillMonstersAndStopSpawn();
				_spawnerIsTurnedOffHere = true;
			} 
		}

		public void Init(IUnit player, List<PlayerView> enemyPlayerViews) {
			_player = player;
			EnemyPlayerViews = enemyPlayerViews;
			_missionResultController.OnPlayerLeftRoomEvent += RefindEnemyPlayerViews;
			_isInited = true;
		}
	
		private void RefindEnemyPlayerViews() {
			EnemyPlayerViews = Object.FindObjectsOfType<PlayerView>().ToList();
			var playerView = _player.UnitView as PlayerView;
			if (playerView != null)
				EnemyPlayerViews.Remove(playerView);
		}
	}

}