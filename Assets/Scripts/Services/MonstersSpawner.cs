using Enums;
using Photon.Pun;
using Units;
using UnityEngine;
using Utils;
using Zenject;


namespace Infrastructure {

	class MonstersSpawner : IMonstersSpawner {
		private const float SPAWN_FROM_SCREEN_OFFSET = 2;
		private readonly IUnitsFactory _unitsFactory;
		private readonly IMonstersMoveController _monstersMoveController;
		private readonly MonstersConfig _monstersConfig;
		private bool _spawnIsOn;
		private readonly IRandomController _random;
		private readonly ICameraController _cameraController;
		private int _spawnerLevel;
		
		private float _leftSpawnPosition;
		private float _rightSpawnPosition;
		private float _bottomSpawnPosition;
		private float _topSpawnPosition;
		
		private float _spawnWaveTimer;
		private Camera _mainCamera;
		private IUnitsPool _unitsPool;


		[Inject]
		public MonstersSpawner(IUnitsFactory unitsFactory, IMonstersMoveController monstersMoveController, IRandomController randomController
				, ICameraController cameraController, IUnitsPool unitsPool, MonstersConfig monstersConfig, IControllersHolder  controllersHolder) {
			_unitsFactory = unitsFactory;
			_monstersMoveController = monstersMoveController;
			_random = randomController;
			_cameraController = cameraController;
			_unitsPool = unitsPool;
			_monstersConfig = monstersConfig;
			_spawnerLevel = 1;
			
			controllersHolder.AddController(this);
		}

		public void OnUpdate(float deltaTime) {
			if (_spawnWaveTimer > 0) {
				_spawnWaveTimer -= deltaTime;
				return;
			}
			
			if (!_spawnIsOn || !_cameraController.CameraIsPositioned)
				return;

			var monstersAmount = _random.GetRandom(_monstersConfig.GetMaxMonstersAmount(_spawnerLevel));
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
			_spawnIsOn = true;
		}

		public void StopSpawn() {
			_spawnIsOn = false;
		}

		private IUnit SpawnMonster() {
			var spawnPosition = GenerateSpawnPosition();
			var currentMonsterLevel = _random.GetRandom(_monstersConfig.GetMaxMonsterLevel(_spawnerLevel) + 1, 1);
			return _unitsPool.SpawnObject(spawnPosition, currentMonsterLevel);


			Vector2 GenerateSpawnPosition() {
				var spawnSide = (ScreenSide)_random.GetRandom((int)ScreenSide.Count, 1);
				var bottomLeftCorner = _mainCamera.ViewportToWorldPoint(Vector3.zero);
				bottomLeftCorner -= new Vector3(SPAWN_FROM_SCREEN_OFFSET, SPAWN_FROM_SCREEN_OFFSET);
				var topRightCorner = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
				topRightCorner += new Vector3(SPAWN_FROM_SCREEN_OFFSET, SPAWN_FROM_SCREEN_OFFSET);
				
				switch (spawnSide) {
					case ScreenSide.Left:
						return new Vector2(bottomLeftCorner.x, _random.GetRandom((int)topRightCorner.y, (int)bottomLeftCorner.y));
					case ScreenSide.Bottom:
						return new Vector2(_random.GetRandom((int)topRightCorner.x, (int)bottomLeftCorner.x), bottomLeftCorner.y);
					case ScreenSide.Right:
						return new Vector2(topRightCorner.x, _random.GetRandom((int)topRightCorner.y, (int)bottomLeftCorner.y));
					case ScreenSide.Top:
						return new Vector2(_random.GetRandom((int)topRightCorner.x, (int)bottomLeftCorner.x), topRightCorner.y);
					case ScreenSide.Count:
					case ScreenSide.None:
					default:
						return Vector2.zero;
				}
			}
		}
	}

}