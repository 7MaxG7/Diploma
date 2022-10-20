using UnityEngine;

namespace Weapons
{
    internal class UpgradedWeaponStats : IWeaponStats
    {
        public WeaponType WeapomType => _baseStats.WeapomType;
        public int[] Damage { get; private set; }
        public float DamageTicksCooldown { get; private set; }
        public float Cooldown { get; private set; }
        public float AmmoSpeed { get; private set; }
        public bool IsPiercing { get; private set; }
        public float SqrRange { get; private set; }
        public float Range { get; private set; }

        private readonly IWeaponStats _baseStats;


        public UpgradedWeaponStats(IWeaponStats baseStats)
        {
            _baseStats = baseStats;
        }

        public override string ToString()
        {
            return
                $"{WeapomType}: Damage = {Damage[0]}, DamageTicksCooldown = {DamageTicksCooldown}, Cooldown = {Cooldown}," +
                $" AmmoSpeed = {AmmoSpeed}, IsPiercing = {IsPiercing}, Range = {Range}";
        }

        public void SetStat(WeaponStatType statType, object statValue)
        {
            switch (statType)
            {
                case WeaponStatType.Damage:
                    Damage = (int[])statValue;
                    break;
                case WeaponStatType.Range:
                    Range = (float)statValue;
                    SqrRange = Range * Range;
                    break;
                case WeaponStatType.Cooldown:
                    Cooldown = (float)statValue;
                    break;
                case WeaponStatType.AmmoSpeed:
                    AmmoSpeed = (float)statValue;
                    break;
                case WeaponStatType.DamageTicksCooldown:
                    DamageTicksCooldown = (float)statValue;
                    break;
                case WeaponStatType.Pierciness:
                    IsPiercing = (bool)statValue;
                    break;
                case WeaponStatType.None:
                default:
                    Debug.LogError($"{this}: unknown weapon stat is trying to be set");
                    break;
            }
        }
    }
}