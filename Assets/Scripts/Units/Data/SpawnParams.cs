using System;


namespace Infrastructure {

	[Serializable]
	internal sealed class SpawnParams {
		public int SpawnerLevel;
		public float SpawnCooldown;
		public int WaveMaxMonstersAmount;
		public int MaxMonsterLevel;
	}

}