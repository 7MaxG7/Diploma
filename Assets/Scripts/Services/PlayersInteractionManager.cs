using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Infrastructure;
using Units;
using UnityEngine;
using Zenject;


namespace Services {

	internal sealed class PlayersInteractionManager : IPlayersInteractionManager {
		public bool IsPlayersFight { get; private set; }
		public PlayerView ClosestFightingEnemyPlayer { get; private set; } 
		public float ClosestFightingEnemyPlayerSqrMagnitude { get; private set; }
		public bool? IsMultiplayerGame { get; private set; }

		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IMissionResultManager _missionResultManager;
		private readonly IControllersHolder _controllersHolder;

		private List<PlayerView> EnemyPlayerViews { get; set; } = new();
		private readonly float _maxPlayersFightSqrMagnitude;
		private IUnit _player;
		private bool _spawnerIsTurnedOffHere;


		[Inject]
		public PlayersInteractionManager(IMonstersSpawner monstersSpawner, IMissionResultManager missionResultManager
				, MissionConfig missionConfig, IControllersHolder controllersHolder) {
			_monstersSpawner = monstersSpawner;
			_missionResultManager = missionResultManager;
			_controllersHolder = controllersHolder;
			_maxPlayersFightSqrMagnitude = missionConfig.PlayersFightDistance * missionConfig.PlayersFightDistance;
		}

		public void Dispose() {
			IsMultiplayerGame = null;
			_missionResultManager.OnPlayerLeftRoomEvent -= RefindEnemyPlayerViews;
			_player = null;
			_controllersHolder.RemoveController(this);
			EnemyPlayerViews.Clear();
		}

		public void OnUpdate(float deltaTime) {
			if (!IsMultiplayerGame.HasValue || !IsMultiplayerGame.Value)
				return;
			
			ClosestFightingEnemyPlayer = null;
			ClosestFightingEnemyPlayerSqrMagnitude = float.MaxValue;
			foreach (var enemyPlayerView in EnemyPlayerViews.Where(view => view != null)) {
				var enemyPlayerSqrDistance = Vector3.SqrMagnitude(enemyPlayerView.Transform.position - _player.Transform.position);
				if (enemyPlayerSqrDistance < ClosestFightingEnemyPlayerSqrMagnitude) {
					ClosestFightingEnemyPlayer = enemyPlayerView;
					ClosestFightingEnemyPlayerSqrMagnitude = enemyPlayerSqrDistance;
				}
			}
			IsPlayersFight = ClosestFightingEnemyPlayerSqrMagnitude <= _maxPlayersFightSqrMagnitude;
			
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
			if (enemyPlayerViews.Count == 0) {
				IsMultiplayerGame = false;
				return;
			}
			
			_player = player;
			EnemyPlayerViews = enemyPlayerViews;
			_missionResultManager.OnPlayerLeftRoomEvent += RefindEnemyPlayerViews;
			IsMultiplayerGame = true;
			_controllersHolder.AddController(this);
		}
		
		private void RefindEnemyPlayerViews() {
			EnemyPlayerViews = Object.FindObjectsOfType<PlayerView>().ToList();
			var playerView = _player.UnitView as PlayerView;
			if (playerView != null)
				EnemyPlayerViews.Remove(playerView);
		}
	}

}