using UnityEngine;


namespace Infrastructure {

	internal interface IAmmosFactory {
		IAmmo CreateAmmo(Vector2 spawnPosition, WeaponType weaponType);
		void SetAmmosPool(IAmmosPool ammosPool);
	}

}