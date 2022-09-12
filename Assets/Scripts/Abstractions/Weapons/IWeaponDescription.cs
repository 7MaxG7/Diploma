namespace Weapons {

	internal interface IWeaponDescription {
		string GetNameForLevel(int level);
		string GetDescriptionForLevel(int level);
	}

}