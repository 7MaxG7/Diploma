using System;


namespace Infrastructure {

	[Serializable]
	internal class SpawnParams {
		public int SpawnerLevel;
		public float SpawnCooldown;
		public int WaveMaxMonstersAmount;
		public int MaxMonsterLevel;
	}

}