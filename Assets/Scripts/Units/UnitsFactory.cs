using Infrastructure;
using Services;
using Units;
using Unity.Mathematics;
using UnityEngine;
using Zenject;


namespace Utils {

	internal sealed class UnitsFactory : IUnitsFactory {
		private readonly IViewsFactory _viewsFactory;
		private readonly PlayerConfig _playerConfig;
		private readonly MonstersConfig _monstersConfig;
		private PlayerUnit _player;


		[Inject]
		public UnitsFactory(IViewsFactory viewsFactory, PlayerConfig playerConfig, MonstersConfig monstersConfig) {
			_viewsFactory = viewsFactory;
			_playerConfig = playerConfig;
			_monstersConfig = monstersConfig;
		}

		public void Dispose() {
			_player.Dispose();
		}

		public IUnit CreatePlayer(Vector2 position) {
			var playerGO = _viewsFactory.CreatePhotonObj(_playerConfig.PlayerPrefabPath, position, quaternion.identity);
			_player = new PlayerUnit(playerGO, _viewsFactory, _playerConfig);
			return _player;
		}

		public IUnit CreateMonster(int monsterLevel, Vector2 spawnPosition) {
			var monsterParams = _monstersConfig.GetMonsterParams(monsterLevel);
			var enemyGO = _viewsFactory.CreatePhotonObj(monsterParams.PrefabPath, spawnPosition, Quaternion.identity);
			return new MonsterUnit(enemyGO, monsterParams, monsterLevel, _viewsFactory);
		}

	}

}