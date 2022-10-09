using Weapons;


namespace Units {

	internal sealed class ActualSkillInfo {
		public WeaponType WeaponType { get; }
		public string SkillName { get; private set; }
		public int Level { get; }
		public string SkillDescription { get; private set; }

		
		public ActualSkillInfo(WeaponType weaponType, int level) {
			WeaponType = weaponType;
			Level = level;
		}

		public void Setup(IWeaponDescription weaponDescription) {
			SkillName = weaponDescription.GetNameForLevel(Level);
			SkillDescription = weaponDescription.GetDescriptionForLevel(Level);
		}
	}

}