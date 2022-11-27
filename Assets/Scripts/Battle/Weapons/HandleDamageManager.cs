using System.Collections.Generic;
using System.Linq;
using Units;
using Utils;
using Zenject;


namespace Infrastructure
{
    internal sealed class HandleDamageManager : IHandleDamageManager
    {
        private readonly IPunEventRaiser _punEventRaiser;

        private List<ComingDamage> _comingDamages = new();


        [Inject]
        public HandleDamageManager(IPunEventRaiser punEventRaiser, IControllersHolder controllersHolder)
        {
            _punEventRaiser = punEventRaiser;
            controllersHolder.AddController(this);
        }

        public void OnUpdate(float deltaTime)
        {
            _comingDamages = _comingDamages.Where(item => item.DamageTaker != null).ToList();
            var damagesToRemove = new List<ComingDamage>();
            foreach (var comingDamage in _comingDamages)
            {
                if (comingDamage.IsCanceled)
                {
                    damagesToRemove.Add(comingDamage);
                    continue;
                }

                comingDamage.ReduceDelay(deltaTime);
                if (comingDamage.IsReady)
                {
                    comingDamage.DamageTaker.TakeDamage(comingDamage.Damage, comingDamage.Damager);
                    if (comingDamage.DamageTaker is PlayerView enemyPlayer &&
                        comingDamage.DamageTaker != comingDamage.Damager.UnitView)
                    {
                        _punEventRaiser.RaiseDamageReceiving(enemyPlayer.PhotonView.ViewID, comingDamage.Damage);
                    }

                    damagesToRemove.Add(comingDamage);
                }
            }

            foreach (var damage in damagesToRemove)
            {
                _comingDamages.Remove(damage);
            }
        }

        public void DealDamage(IDamagableView damageTaker, int[] damage, float damageTicksCooldown, IUnit damager)
        {
            if (damage.Length == 1)
                DealPermanentDamage(damageTaker, damage[0], damager);
            else if (damage.Length > 1)
            {
                DeadPeriodicalDamage(damageTaker, damage, damageTicksCooldown, damager);
            }
        }

        private void DealPermanentDamage(IDamagableView damageTaker, int damage, IUnit damager)
        {
            damageTaker.TakeDamage(damage, damager);
            if (damageTaker is PlayerView enemyPlayer && damageTaker != damager.UnitView)
            {
                _punEventRaiser.RaiseDamageReceiving(enemyPlayer.PhotonView.ViewID, damage);
            }
        }

        private void DeadPeriodicalDamage(IDamagableView damageTaker, int[] damage, float damageTicksCooldown,
            IUnit damager)
        {
            float currentCooldown = 0;
            foreach (var tickDamage in damage)
            {
                _comingDamages.Add(new ComingDamage(currentCooldown, damageTaker, tickDamage, damager));
                currentCooldown += damageTicksCooldown;
            }
        }

        public void StopPeriodicalDamageForUnit(IUnit unit)
        {
            var damageTaker = unit.UnitView as IDamagableView;
            foreach (var comingDamage in _comingDamages.Where(damage => damage.DamageTaker == damageTaker))
            {
                comingDamage.StopDamage();
            }
        }
    }
}