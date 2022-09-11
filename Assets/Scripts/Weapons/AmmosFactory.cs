using Services;
using UnityEngine;
using Zenject;


namespace Infrastructure {

	internal sealed class AmmosFactory : IAmmosFactory {
		private readonly IHandleDamageManager _handleDamageManager;
		private readonly IViewsFactory _viewsFactory;
		private readonly WeaponsConfig _weaponsConfig;
		private IAmmosPool _ammosPool;

		
		[Inject]
		public AmmosFactory(IHandleDamageManager handleDamageManager, IViewsFactory viewsFactory, WeaponsConfig weaponsConfig) {
			_handleDamageManager = handleDamageManager;
			_viewsFactory = viewsFactory;
			_weaponsConfig = weaponsConfig;
		}
		
		public IAmmo CreateAmmo(Vector2 position, WeaponType weaponType) {
			var ammoParam = _weaponsConfig.GetWeaponBaseParam(weaponType);
			var ammoGo = _viewsFactory.CreatePhotonObj(ammoParam.AmmoPrefabPath, position, Quaternion.identity);
			return new Ammo(ammoGo, _ammosPool, _handleDamageManager, _viewsFactory, (int)weaponType);
		}

		public void SetAmmosPool(IAmmosPool ammosPool) {
			_ammosPool = ammosPool;
		}
	}

}