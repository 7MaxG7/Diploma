using Services;
using UnityEngine;
using Zenject;


namespace Infrastructure {

	internal class AmmosPool : ObjectsPool<IAmmo>, IAmmosPool {
		private readonly IAmmosFactory _ammosFactory;

		
		[Inject]
		public AmmosPool(IAmmosFactory ammosFactory) {
			_ammosFactory = ammosFactory;
			_ammosFactory.SetAmmosPool(this);
		}
		
		protected override IAmmo SpawnSpecifiedObject(Vector2 spawnPosition, object[] parameters) {
			var ammoType = (WeaponType)parameters[0];
			return _ammosFactory.CreateAmmo(spawnPosition, ammoType);
		}
	}

}