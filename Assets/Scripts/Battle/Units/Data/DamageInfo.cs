namespace Units
{
    internal sealed class DamageInfo
    {
        public int Damage { get; }
        public IUnit Damager { get; }
        public IUnit DamageTaker { get; }


        public DamageInfo(int damage, IUnit damager, IUnit damageTaker)
        {
            Damage = damage;
            Damager = damager;
            DamageTaker = damageTaker;
        }
    }
}