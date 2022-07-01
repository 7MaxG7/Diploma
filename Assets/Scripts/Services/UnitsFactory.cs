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
		private IUnitsPool _unitsPool;


		[Inject]
		public UnitsFactory(PlayerConfig playerConfig, MonstersConfig monstersConfig) {
			_playerConfig = playerConfig;
			_monstersConfig = monstersConfig;
		}

		public IUnit CreatePlayer(Vector2 position) {
			var playerGO = PhotonNetwork.Instantiate(TextConstants.PLAYER_PREF_RESOURCES_PATH, position, quaternion.identity);
			return new PlayerUnit(playerGO, _playerConfig.BaseMoveSpeed, _playerConfig.BaseHp);
		}

		public IUnit CreateMonster(int monsterLevel, Vector2 spawnPosition) {
			var monsterParams = _monstersConfig.GetMonsterParams(monsterLevel);
			var enemyGO = PhotonNetwork.Instantiate(monsterParams.PrefabPath, spawnPosition, Quaternion.identity);
			return new MonsterUnit(enemyGO, monsterParams, _unitsPool);
		}

		public void SetUnitsPool(IUnitsPool unitsPool) {
			_unitsPool = unitsPool;
		}
	}

}