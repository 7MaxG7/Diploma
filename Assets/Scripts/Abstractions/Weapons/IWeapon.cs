using System;
using UnityEngine;


namespace Weapons {

	internal interface IWeapon {
		event Action<WeaponType> OnShooted;
		
		float SqrRange { get; }
		bool IsReady { get; }
		WeaponType Type { get; }
		int Level { get; }

		void OnDispose();
		void ReduceCooldown(float deltaTime);
		void Shoot(Vector3 targetPosition);
		void Upgrade(WeaponLevelUpgradeParam upgradeParam);
	}

}