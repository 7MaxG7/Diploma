using Photon.Pun;
using UnityEngine;
using Zenject;


namespace Infrastructure {

	class AmmosFactory : IAmmosFactory {
		private readonly IHandleDamageController _handleDamageController;
		private readonly WeaponsConfig _weaponsConfig;
		private IAmmosPool _ammosPool;

		[Inject]
		public AmmosFactory(IHandleDamageController handleDamageController, WeaponsConfig weaponsConfig) {
			_handleDamageController = handleDamageController;
			_weaponsConfig = weaponsConfig;
		}
		
		public IAmmo CreateAmmo(Vector2 position, WeaponType weaponType) {
			var ammoParam = _weaponsConfig.GetWeaponBaseParam(weaponType);
			var ammoGo = PhotonNetwork.Instantiate(ammoParam.AmmoPrefabPath, position, Quaternion.identity);
			return new Ammo(ammoGo, _ammosPool, _handleDamageController, (int)weaponType);
		}

		public void SetAmmosPool(IAmmosPool ammosPool) {
			_ammosPool = ammosPool;
		}
	}

}