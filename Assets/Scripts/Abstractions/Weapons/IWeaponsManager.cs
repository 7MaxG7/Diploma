using System;
using System.Collections.Generic;
using Infrastructure;
using Units;


namespace Weapons
{
    internal interface IWeaponsManager : IUpdater, IDisposable
    {
        List<WeaponType> UpgradableWeaponTypes { get; }

        void Init(IUnit player);
        void StopShooting();
        void StartShooting();
        void AddWeapon(WeaponType type);
        int GetCurrentLevelOfWeapon(WeaponType weaponType);
        void AddOrUpgradeWeapon(WeaponType weaponType);
    }
}