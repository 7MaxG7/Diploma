using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Services;
using UnityEngine;
using Utils;
using Zenject;


namespace Weapons
{
    internal sealed class AmmosPool : ObjectsPool<IAmmo>, IAmmosPool
    {
        private readonly IAmmosFactory _ammosFactory;
        private readonly IHandleDamageManager _handleDamageManager;


        [Inject]
        public AmmosPool(IAmmosFactory ammosFactory, IHandleDamageManager handleDamageManager,
            IPhotonManager photonManager, IPunEventRaiser punEventRaiser)
        {
            _ammosFactory = ammosFactory;
            _handleDamageManager = handleDamageManager;
            PunEventRaiser = punEventRaiser;
            PhotonManager = photonManager;
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var ammo in Objects.Values.SelectMany(obj => obj))
            {
                ammo.OnLifetimeExpired -= ReturnObject;
                ammo.OnCollidedWithDamageTaker -= _handleDamageManager.DealDamage;
                ammo.Dispose();
            }

            foreach (var objList in Objects.Values)
            {
                objList.Clear();
            }

            Objects.Clear();
            foreach (var ammo in SpawnedObjects)
            {
                ammo.OnLifetimeExpired -= ReturnObject;
                ammo.OnCollidedWithDamageTaker -= _handleDamageManager.DealDamage;
                ammo.Dispose();
            }

            SpawnedObjects.Clear();
        }

        protected override int GetSpecifiedPoolIndex(object[] parameters)
        {
            return (int)parameters[0];
        }

        protected override async Task<IAmmo> SpawnSpecifiedObjectAsync(Vector2 position, Quaternion rotation,
            object[] parameters)
        {
            var ammoType = (WeaponType)parameters[0];
            var ammo = await _ammosFactory.CreateMyAmmoAsync(ammoType, position, rotation);
            ammo.OnLifetimeExpired += ReturnObject;
            ammo.OnCollidedWithDamageTaker += _handleDamageManager.DealDamage;
            return ammo;
        }
    }
}