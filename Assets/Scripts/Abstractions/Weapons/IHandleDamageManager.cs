using Units;


namespace Infrastructure
{
    internal interface IHandleDamageManager : IUpdater
    {
        void DealDamage(IDamagableView damageTaker, int[] damage, float damageTicksCooldown, IUnit damager);
        void StopPeriodicalDamageForUnit(IUnit unit);
    }
}