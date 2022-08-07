using System.Linq;
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

		public void Dispose() {
			foreach (var ammo in _objects.Values.SelectMany(obj => obj)) {
				ammo.Dispose();
			}
			foreach (var objList in _objects.Values) {
				objList.Clear();
			}
			_objects.Clear();
			foreach (var ammo in _spawnedObjects) {
				ammo.Dispose();
			}
			_spawnedObjects.Clear();
		}

		protected override int GetSpecifiedPoolIndex(object[] parameters) {
			return (int)parameters[0];
		}

		protected override IAmmo SpawnSpecifiedObject(Vector2 spawnPosition, object[] parameters) {
			var ammoType = (WeaponType)parameters[0];
			return _ammosFactory.CreateAmmo(spawnPosition, ammoType);
		}

	}

}