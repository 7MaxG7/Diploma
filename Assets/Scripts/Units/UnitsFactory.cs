using Infrastructure;
using Photon.Pun;
using Units;
using Unity.Mathematics;
using UnityEngine;
using Zenject;


namespace Utils {

	internal class UnitsFactory : IUnitsFactory {
		private readonly PlayerConfig _playerConfig;
		private readonly MonstersConfig _monstersConfig;


		[Inject]
		public UnitsFactory(PlayerConfig playerConfig, MonstersConfig monstersConfig) {
			_playerConfig = playerConfig;
			_monstersConfig = monstersConfig;
		}

		public IUnit CreatePlayer(Vector2 position) {
			var playerGO = PhotonNetwork.Instantiate(_playerConfig.PlayerPrefabPath, position, quaternion.identity);
			return new PlayerUnit(playerGO, _playerConfig);
		}

		public IUnit CreateMonster(int monsterLevel, Vector2 spawnPosition) {
			var monsterParams = _monstersConfig.GetMonsterParams(monsterLevel);
			var enemyGO = PhotonNetwork.Instantiate(monsterParams.PrefabPath, spawnPosition, Quaternion.identity);
			return new MonsterUnit(enemyGO, monsterParams);
		}
	}

}