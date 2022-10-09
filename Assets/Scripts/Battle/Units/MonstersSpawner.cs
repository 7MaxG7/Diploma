using Enums;
using Infrastructure;
using Units;
using UnityEngine;
using Zenject;


namespace Services {

	internal sealed class MonstersSpawner : IMonstersSpawner {
		public bool SpawnIsOn { get; private set; }
		
		private readonly MonstersConfig _monstersConfig;
		private readonly IMonstersMoveManager _monstersMoveManager;
		private readonly IRandomManager _random;
		private readonly ICameraManager _cameraManager;
		private readonly IUnitsPool _unitsPool;
		private readonly int _maxSpawnerLevel;
		private Camera _mainCamera;

		private float _leftSpawnPosition;
		private float _rightSpawnPosition;
		private float _bottomSpawnPosition;
		private float _topSpawnPosition;
		
		private int _spawnerLevel;
		private float _spawnWaveTimer;


		[Inject]
		public MonstersSpawner(IMonstersMoveManager monstersMoveManager, IRandomManager randomManager
				, ICameraManager cameraManager, IUnitsPool unitsPool, MonstersConfig monstersConfig, IControllersHolder  controllersHolder) {
			_monstersMoveManager = monstersMoveManager;
			_random = randomManager;
			_cameraManager = cameraManager;
			_unitsPool = unitsPool;
			_monstersConfig = monstersConfig;
			_maxSpawnerLevel = _monstersConfig.GetMaxSpawnerLevel();

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
			
			if (!SpawnIsOn || !_cameraManager.CameraIsPositioned)
				return;

			var monstersAmount = _unitsPool.ActiveMonsters.Count > 0
					? _random.GetRandomIncludingMax(_monstersConfig.GetMaxMonstersAmount(_spawnerLevel)) 
					: _random.GetRandomIncludingMax(_monstersConfig.GetMaxMonstersAmount(_spawnerLevel), 1);
			for (var i = 0; i < monstersAmount; i++) {
				var monster = SpawnMonster();
				_monstersMoveManager.RegisterMonster(monster);
			}

			_spawnWaveTimer = _monstersConfig.GetSpawnCooldown(_spawnerLevel);
		}

		public void Init(IUnit player) {
			_mainCamera = Camera.main;
			_spawnerLevel = 1;
			player.Experience.OnLevelUp += IncreaseSpawnerLevel;
		}

		private void IncreaseSpawnerLevel(int _) {
			if (_spawnerLevel >= _maxSpawnerLevel)
				return;
			
			_spawnerLevel++;
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
				bottomLeftCorner -= new Vector3(_monstersConfig.HorizontalSpawnFromScreenOffset, _monstersConfig.VerticalSpawnFromScreenOffset);
				var topRightCorner = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
				topRightCorner += new Vector3(_monstersConfig.HorizontalSpawnFromScreenOffset, _monstersConfig.VerticalSpawnFromScreenOffset);
				
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