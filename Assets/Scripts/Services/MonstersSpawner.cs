using Enums;
using Infrastructure;
using Units;
using UnityEngine;
using Zenject;


namespace Services {

	internal class MonstersSpawner : IMonstersSpawner {
		private const float SPAWN_FROM_SCREEN_OFFSET = 2;
		
		public bool SpawnIsOn { get; private set; }
		
		private readonly MonstersConfig _monstersConfig;
		private readonly IMonstersMoveController _monstersMoveController;
		private readonly IRandomController _random;
		private readonly ICameraController _cameraController;
		private readonly IUnitsPool _unitsPool;
		private int _spawnerLevel;
		private Camera _mainCamera;

		private float _leftSpawnPosition;
		private float _rightSpawnPosition;
		private float _bottomSpawnPosition;
		private float _topSpawnPosition;
		
		private float _spawnWaveTimer;


		[Inject]
		public MonstersSpawner(IMonstersMoveController monstersMoveController, IRandomController randomController
				, ICameraController cameraController, IUnitsPool unitsPool, MonstersConfig monstersConfig, IControllersHolder  controllersHolder) {
			_monstersMoveController = monstersMoveController;
			_random = randomController;
			_cameraController = cameraController;
			_unitsPool = unitsPool;
			_monstersConfig = monstersConfig;
			_spawnerLevel = 1;
			
			controllersHolder.AddController(this);
		}

		public void Dispose() {
			KillMonstersAndStopSpawn();
			_mainCamera = null;
		}

		public void OnUpdate(float deltaTime) {
			if (_spawnWaveTimer > 0) {
				_spawnWaveTimer -= deltaTime;
				return;
			}
			
			if (!SpawnIsOn || !_cameraController.CameraIsPositioned)
				return;

			var monstersAmount = _random.GetRandomIncludingMax(_monstersConfig.GetMaxMonstersAmount(_spawnerLevel));
			for (var i = 0; i < monstersAmount; i++) {
				var monster = SpawnMonster();
				_monstersMoveController.RegisterMonster(monster);
			}

			_spawnWaveTimer = _monstersConfig.GetSpawnCooldown(_spawnerLevel);
		}

		public void Init() {
			_mainCamera = Camera.main;
		}

		public void StartSpawn() {
			SpawnIsOn = true;
		}

		public void KillMonstersAndStopSpawn() {
			SpawnIsOn = false;
			for (var i = _unitsPool.ActiveMonsters.Count - 1; i >= 0; i--) {
				_unitsPool.ActiveMonsters[i].KillUnit();
			}
		}

		private IUnit SpawnMonster() {
			var spawnPosition = GenerateSpawnPosition();
			var currentMonsterLevel = _random.GetRandomIncludingMax(_monstersConfig.GetMaxMonsterLevel(_spawnerLevel), 1);
			return _unitsPool.SpawnObject(spawnPosition, currentMonsterLevel);


			Vector2 GenerateSpawnPosition() {
				var spawnSide = (ScreenSide)_random.GetRandomExcludingMax((int)ScreenSide.Count, 1);
				var bottomLeftCorner = _mainCamera.ViewportToWorldPoint(Vector3.zero);
				bottomLeftCorner -= new Vector3(SPAWN_FROM_SCREEN_OFFSET, SPAWN_FROM_SCREEN_OFFSET);
				var topRightCorner = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
				topRightCorner += new Vector3(SPAWN_FROM_SCREEN_OFFSET, SPAWN_FROM_SCREEN_OFFSET);
				
				switch (spawnSide) {
					case ScreenSide.Left:
						return new Vector2(bottomLeftCorner.x, _random.GetRandomIncludingMax((int)topRightCorner.y, (int)bottomLeftCorner.y));
					case ScreenSide.Bottom:
						return new Vector2(_random.GetRandomIncludingMax((int)topRightCorner.x, (int)bottomLeftCorner.x), bottomLeftCorner.y);
					case ScreenSide.Right:
						return new Vector2(topRightCorner.x, _random.GetRandomIncludingMax((int)topRightCorner.y, (int)bottomLeftCorner.y));
					case ScreenSide.Top:
						return new Vector2(_random.GetRandomIncludingMax((int)topRightCorner.x, (int)bottomLeftCorner.x), topRightCorner.y);
					case ScreenSide.Count:
					case ScreenSide.None:
					default:
						return Vector2.zero;
				}
			}
		}
	}

}