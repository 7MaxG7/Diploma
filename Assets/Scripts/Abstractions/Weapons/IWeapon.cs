using Units;


namespace Infrastructure {

	internal interface IWeapon {
		float SqrRange { get; }
		bool IsReady { get; }
		WeaponType Type { get; }
		int Level { get; }

		void ReduceCooldown(float deltaTime);
		void Shoot(IUnit target);
		void Upgrade(WeaponLevelUpgradeParam upgradeParam);
	}

}