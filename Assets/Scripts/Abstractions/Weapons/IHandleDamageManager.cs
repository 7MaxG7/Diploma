using System;
using Services;
using Units;


namespace Infrastructure
{
    internal interface IHandleDamageManager : IUpdater
    {
        event Action<PhotonDamageInfo> OnDamageEnemyPlayer;

        void DealDamage(IDamagableView damageTaker, int[] damage, float damageTicksCooldown, IUnit damager);
        void StopPeriodicalDamageForUnit(IUnit unit);
    }
}