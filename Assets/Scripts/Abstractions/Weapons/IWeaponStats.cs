namespace Weapons
{
    internal interface IWeaponStats
    {
        int[] Damage { get; }
        float Range { get; }
        float SqrRange { get; }
        float Cooldown { get; }
        bool IsPiercing { get; }
        float AmmoSpeed { get; }
        float DamageTicksCooldown { get; }
        WeaponType WeapomType { get; }
    }
}