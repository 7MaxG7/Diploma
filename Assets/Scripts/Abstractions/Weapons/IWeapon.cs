using System;
using UnityEngine;


namespace Infrastructure {

	internal interface IWeapon {
		float SqrRange { get; }
		bool IsReady { get; }
		WeaponType Type { get; }
		int Level { get; }

		void ReduceCooldown(float deltaTime);
		void Shoot(Vector3 targetPosition);
		void Upgrade(WeaponLevelUpgradeParam upgradeParam);

		event Action<WeaponType> OnShooted;
	}

}