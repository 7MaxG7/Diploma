namespace Infrastructure {

	internal interface IWeaponDescription {
		string GetNameForLevel(int level);
		string GetDescriptionForLevel(int level);
	}

}