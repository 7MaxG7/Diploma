using Services;
using UnityEngine;
using Zenject;


namespace Weapons {

	internal sealed class AmmosFactory : IAmmosFactory {
		private readonly IViewsFactory _viewsFactory;
		private readonly WeaponsConfig _weaponsConfig;

		
		[Inject]
		public AmmosFactory(IViewsFactory viewsFactory, WeaponsConfig weaponsConfig) {
			_viewsFactory = viewsFactory;
			_weaponsConfig = weaponsConfig;
		}
		
		public IAmmo CreateAmmo(Vector2 position, WeaponType weaponType) {
			var ammoParam = _weaponsConfig.GetWeaponBaseParam(weaponType);
			var ammoGo = _viewsFactory.CreatePhotonObj(ammoParam.AmmoPrefabPath, position, Quaternion.identity);
			var ammo = new Ammo(ammoGo, (int)weaponType);
			return ammo;
		}
	}

}