namespace Units
{
    internal interface IDamagableView
    {
        void TakeDamage(int damage, IUnit damager);
    }
}