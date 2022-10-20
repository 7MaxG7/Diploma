using Weapons;

namespace Utils
{
    internal interface IUpgradedWeaponsFactory
    {
        IWeaponStats CreatedUpgradeWeaponStats(IWeaponStats baseStats, WeaponLevelUpgradeParam upgradeParam);
    }
}