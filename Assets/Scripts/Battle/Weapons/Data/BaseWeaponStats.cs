using System;

namespace Weapons
{
    internal class BaseWeaponStats : IWeaponStats
    {
        public WeaponType WeapomType { get; }
        public int[] Damage { get; }
        public float Range { get; }
        public float SqrRange { get; }
        public float Cooldown { get; }
        public bool IsPiercing { get; }
        public float AmmoSpeed { get; }
        public float DamageTicksCooldown { get; }

		
        public BaseWeaponStats(WeaponBaseParam weaponBaseParam)
        {
            WeapomType = weaponBaseParam.WeaponType;
            Damage = new int[weaponBaseParam.BaseTicksDamage.Length];
            Array.Copy(weaponBaseParam.BaseTicksDamage, Damage, weaponBaseParam.BaseTicksDamage.Length);
            Range = weaponBaseParam.Range;
            SqrRange = Range * Range;
            Cooldown = weaponBaseParam.Cooldown;
            DamageTicksCooldown = weaponBaseParam.DamageTicksCooldown;
            AmmoSpeed = weaponBaseParam.AmmoSpeed;
            IsPiercing = weaponBaseParam.IsPiercing;
        }

        public override string ToString()
        {
            return $"{WeapomType}: Damage = {Damage[0]}, DamageTicksCooldown = {DamageTicksCooldown}, Cooldown = {Cooldown}," +
                   $" AmmoSpeed = {AmmoSpeed}, IsPiercing = {IsPiercing}, Range = {Range}";
        }
    }
}