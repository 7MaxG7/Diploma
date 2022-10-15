using System.Linq;
using Abstractions.Utils;
using UnityEngine;
using Utils.WeaponMathHandler;
using Weapons;

namespace Utils
{
    internal class UpgradedWeaponsFactory : IUpgradedWeaponsFactory
    {
        public IWeaponStats CreatedUpgradeWeaponStats(IWeaponStats baseStats, WeaponLevelUpgradeParam upgradeParam)
        {
            var upgradedStats = new UpgradedWeaponStats(baseStats);
            foreach (var upgrade in upgradeParam.Upgrades)
            {
                ApplyUpgrade(baseStats, upgradedStats, upgrade);
            }
            var upgradedTypes = upgradeParam.Upgrades.Select(upgrade => upgrade.StatType).ToArray();
            SetUnupgradedStats(upgradedTypes, baseStats, upgradedStats);
            return upgradedStats;
        }

        private void ApplyUpgrade(IWeaponStats baseStats, UpgradedWeaponStats upgradedWeaponStats,
            UpgradeExpresstion upgrade)
        {
            var mathHandler = CreateMathHandler(upgrade.StatType);
            switch (upgrade.StatType)
            {
                case WeaponStatType.Damage:
                    var damage = mathHandler.CountUpgradedStat(baseStats.Damage, upgrade.Arithmetic, upgrade.DeltaValue);
                    upgradedWeaponStats.SetStat(upgrade.StatType, damage);
                    break;
                case WeaponStatType.Range:
                    var range = mathHandler.CountUpgradedStat(baseStats.Range, upgrade.Arithmetic, upgrade.DeltaValue);
                    upgradedWeaponStats.SetStat(upgrade.StatType, range);
                    break;
                case WeaponStatType.Cooldown:
                    var cooldown = mathHandler.CountUpgradedStat(baseStats.Cooldown, upgrade.Arithmetic, upgrade.DeltaValue);
                    upgradedWeaponStats.SetStat(upgrade.StatType, cooldown);
                    break;
                case WeaponStatType.AmmoSpeed:
                    var ammoSpeed = mathHandler.CountUpgradedStat(baseStats.AmmoSpeed, upgrade.Arithmetic, upgrade.DeltaValue);
                    upgradedWeaponStats.SetStat(upgrade.StatType, ammoSpeed);
                    break;
                case WeaponStatType.DamageTicksCooldown:
                    var damageTicksCooldown = mathHandler.CountUpgradedStat(baseStats.DamageTicksCooldown, upgrade.Arithmetic, upgrade.DeltaValue);
                    upgradedWeaponStats.SetStat(upgrade.StatType, damageTicksCooldown);
                    break;
                case WeaponStatType.Pierciness:
                    var isPiercing = mathHandler.CountUpgradedStat(baseStats.IsPiercing, upgrade.Arithmetic, upgrade.DeltaValue);
                    upgradedWeaponStats.SetStat(upgrade.StatType, isPiercing);
                    break;
                case WeaponStatType.None:
                default:
                    return;
            }
        }

        private IWeaponMathHandler CreateMathHandler(WeaponStatType statType)
        {
            switch (statType)
            {
                case WeaponStatType.Damage:
                    return new IntArrayWeaponMathHandler();
                case WeaponStatType.Range:
                case WeaponStatType.Cooldown:
                case WeaponStatType.AmmoSpeed:
                case WeaponStatType.DamageTicksCooldown:
                    return new FloatWeaponMathHandler();
                case WeaponStatType.Pierciness:
                    return new BoolWeaponMathHandler();
                case WeaponStatType.None:
                default:
                    Debug.LogError($"{this}: Unknown weapon characteristic type");
                    return null;
            }
        }
  
        private void SetUnupgradedStats(WeaponStatType[] upgradedTypes, IWeaponStats baseStats
            , UpgradedWeaponStats upgradedWeaponStats)
        {
            if (!upgradedTypes.Contains(WeaponStatType.Damage))
            {
                var damage = new int[baseStats.Damage.Length];
                baseStats.Damage.CopyTo(damage, 0);
                upgradedWeaponStats.SetStat(WeaponStatType.Damage, damage);
            }
            if (!upgradedTypes.Contains(WeaponStatType.Range))
            {
                var range = baseStats.Range;
                upgradedWeaponStats.SetStat(WeaponStatType.Range, range);
            }
            if (!upgradedTypes.Contains(WeaponStatType.Cooldown))
            {
                var cooldown = baseStats.Cooldown;
                upgradedWeaponStats.SetStat(WeaponStatType.Cooldown, cooldown);
            }
            if (!upgradedTypes.Contains(WeaponStatType.AmmoSpeed))
            {
                var ammoSpeed = baseStats.AmmoSpeed;
                upgradedWeaponStats.SetStat(WeaponStatType.AmmoSpeed, ammoSpeed);
            }
            if (!upgradedTypes.Contains(WeaponStatType.DamageTicksCooldown))
            {
                var damageTickCooldown = baseStats.DamageTicksCooldown;
                upgradedWeaponStats.SetStat(WeaponStatType.DamageTicksCooldown, damageTickCooldown);
            }
            if (!upgradedTypes.Contains(WeaponStatType.Pierciness))
            {
                var isPiercing = baseStats.IsPiercing;
                upgradedWeaponStats.SetStat(WeaponStatType.Pierciness, isPiercing);
            }
        }
    }
}