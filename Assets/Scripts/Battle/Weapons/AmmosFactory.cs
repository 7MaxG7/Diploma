using System.Threading.Tasks;
using Services;
using UnityEngine;
using Utils;
using Zenject;


namespace Weapons
{
    internal sealed class AmmosFactory : IAmmosFactory
    {
        private readonly IViewsFactory _viewsFactory;
        private readonly IPunEventRaiser _punEventRaiser;
        private readonly WeaponsConfig _weaponsConfig;
        private Transform _root;


        [Inject]
        public AmmosFactory(IViewsFactory viewsFactory, IPunEventRaiser punEventRaiser, WeaponsConfig weaponsConfig)
        {
            _viewsFactory = viewsFactory;
            _punEventRaiser = punEventRaiser;
            _weaponsConfig = weaponsConfig;
        }

        public void Init()
        {
            _root = _viewsFactory.CreateGameObject(Constants.AMMOS_ROOT_NAME).transform;
        }

        public async Task<IAmmo> CreateMyAmmoAsync(WeaponType weaponType, Vector2 position, Quaternion rotation)
        {
            var ammo = await CreateAmmoAsync(weaponType, position, rotation, true);
            _punEventRaiser.RaiseAmmoCreation(ammo.PhotonView, ammo.Transform.position, ammo.Transform.rotation,
                (int)weaponType);
            return ammo;
        }

        public async Task<IAmmo> CreateAmmoAsync(WeaponType weaponType, Vector2 position, Quaternion rotation, bool isMine = false)
        {
            var ammoParam = _weaponsConfig.GetWeaponBaseParam(weaponType);
            var ammoGo = await _viewsFactory.CreateGameObjectAsync(ammoParam.AmmoPrefab, position, rotation, _root);
            var ammo = new Ammo(ammoGo, (int)weaponType, isMine);
            return ammo;
        }
    }
}