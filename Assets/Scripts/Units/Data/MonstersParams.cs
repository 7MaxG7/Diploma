using System;


namespace Infrastructure {

	[Serializable]
	internal sealed class MonstersParams {
		public int MonsterLevel;
		public string PrefabPath;
		public float MoveSpeed;
		public int Hp;
		public int Damage;
		public int ExperienceOnKill;
	}

}