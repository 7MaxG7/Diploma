using UnityEngine;


namespace Weapons {

	internal interface IAmmosFactory {
		IAmmo CreateAmmo(Vector2 spawnPosition, WeaponType weaponType);
	}

}