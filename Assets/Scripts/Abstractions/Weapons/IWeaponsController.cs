﻿using System.Collections.Generic;
using Units;


namespace Infrastructure {

	internal interface IWeaponsController : IUpdater {
		List<WeaponType> UpgradableWeaponTypes { get; }
		
		void Init(IUnit player, ISoundController soundController);
		void StopShooting();
		void StartShooting();
		void AddWeapon(WeaponType type);
		int GetCurrentLevelOfWeapon(WeaponType weaponType);
		void AddOrUpgradeWeapon(WeaponType weaponType);
	}

}