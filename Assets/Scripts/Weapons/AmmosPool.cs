using System.Linq;
using Infrastructure;
using Services;
using UnityEngine;
using Zenject;


namespace Weapons {

	internal sealed class AmmosPool : ObjectsPool<IAmmo>, IAmmosPool {
		private readonly IAmmosFactory _ammosFactory;
		private readonly IHandleDamageManager _handleDamageManager;


		[Inject]
		public AmmosPool(IAmmosFactory ammosFactory, IHandleDamageManager handleDamageManager, IViewsFactory viewsFactory) {
			_ammosFactory = ammosFactory;
			_handleDamageManager = handleDamageManager;
			_viewsFactory = viewsFactory;
		}

		public override void Dispose() {
			base.Dispose();
			foreach (var ammo in _objects.Values.SelectMany(obj => obj)) {
				ammo.OnLifetimeExpired -= ReturnObject;
				ammo.OnCollidedWithDamageTaker -= _handleDamageManager.DealDamage;
				ammo.Dispose();
			}
			foreach (var objList in _objects.Values) {
				objList.Clear();
			}
			_objects.Clear();
			foreach (var ammo in _spawnedObjects) {
				ammo.OnLifetimeExpired -= ReturnObject;
				ammo.OnCollidedWithDamageTaker -= _handleDamageManager.DealDamage;
				ammo.Dispose();
			}
			_spawnedObjects.Clear();
		}

		protected override int GetSpecifiedPoolIndex(object[] parameters) {
			return (int)parameters[0];
		}

		protected override IAmmo SpawnSpecifiedObject(Vector2 spawnPosition, object[] parameters) {
			var ammoType = (WeaponType)parameters[0];
			var ammo = _ammosFactory.CreateAmmo(spawnPosition, ammoType);
			ammo.OnLifetimeExpired += ReturnObject;
			ammo.OnCollidedWithDamageTaker += _handleDamageManager.DealDamage;
			return ammo;
		}

	}

}