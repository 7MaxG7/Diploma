using System;
using System.Linq;
using UnityEngine;


namespace Infrastructure {

	[Serializable]
	internal class SpawnParams {
		public int SpawnerLevel;
		public float SpawnCooldown;
		public int WaveMaxMonstersAmount;
		public int MaxMonsterLevel;
	}

	[Serializable]
	internal class MonstersParams {
		public int MonsterLevel;
		public string PrefabPath;
		public float MoveSpeed;
		public int Hp;
		public int Damage;
		public int ExperienceOnKill;
	}

	[CreateAssetMenu(menuName = "Configs/" + nameof(MonstersConfig), fileName = nameof(MonstersConfig), order = 3)]
	internal class MonstersConfig : ScriptableObject {
		[SerializeField] private SpawnParams[] _spawnParams;
		[SerializeField] private MonstersParams[] _monstersParams;
		
		
		public int GetMaxMonstersAmount(int spawnerLevel) {
			return GetParamsForLevel(spawnerLevel).WaveMaxMonstersAmount;
		}

		public int GetMaxMonsterLevel(int spawnerLevel) {
			return GetParamsForLevel(spawnerLevel).MaxMonsterLevel;
		}

		public MonstersParams GetMonsterParams(int enemyLevel) {
			var monsterParams = _monstersParams.FirstOrDefault(enemy => enemy.MonsterLevel == enemyLevel);
			if (monsterParams == null) {
				var maxLevel = _monstersParams.Select(pref => pref.MonsterLevel).Max();
				monsterParams = _monstersParams.First(enemy => enemy.MonsterLevel == maxLevel);
			}
			return monsterParams;
		}

		public float GetSpawnCooldown(int spawnerLevel) {
			return GetParamsForLevel(spawnerLevel).SpawnCooldown;
		}

		private SpawnParams GetParamsForLevel(int spawnerLevel) {
			var currentParam = _spawnParams.FirstOrDefault(param => param.SpawnerLevel == spawnerLevel);
			if (currentParam == null) {
				var maxLevel = _spawnParams.Select(param => param.SpawnerLevel).Max();
				currentParam = _spawnParams.First(param => param.SpawnerLevel == maxLevel);
			}
			return currentParam;
		}

	}

}