using Weapons;

namespace Abstractions.Utils
{
    internal interface IUpgradedWeaponsFactory
    {
        IWeaponStats CreatedUpgradeWeaponStats(IWeaponStats baseStats, WeaponLevelUpgradeParam upgradeParam);
    }
}