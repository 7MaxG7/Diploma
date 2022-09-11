using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using Units;
using Zenject;


namespace Infrastructure {

	internal sealed class HandleDamageManager : IHandleDamageManager {
		public event Action<PhotonDamageInfo> OnDamageEnemyPlayer;
		
		private List<ComingDamage> _comingDamages = new();


		[Inject]
		public HandleDamageManager(IControllersHolder controllersHolder) {
			controllersHolder.AddController(this);
		}

		public void OnUpdate(float deltaTime) {
			_comingDamages = _comingDamages.Where(item => item.DamageTaker != null).ToList();
			var damagesToRemove = new List<ComingDamage>();
			foreach (var comingDamage in _comingDamages) {
				if (comingDamage.IsCanceled) {
					damagesToRemove.Add(comingDamage);
					continue;
				}
				
				comingDamage.ReduceDelay(deltaTime);
				if (comingDamage.IsReady) {
					comingDamage.DamageTaker.TakeDamage(comingDamage.Damage, comingDamage.Damager);
					if (comingDamage.DamageTaker is PlayerView enemyPlayer && enemyPlayer != comingDamage.Damager.UnitView) {
						OnDamageEnemyPlayer?.Invoke(new PhotonDamageInfo(enemyPlayer.PhotonView.ViewID, comingDamage.Damage));
					}
					damagesToRemove.Add(comingDamage);
				}
			}
			foreach (var damage in damagesToRemove) {
				_comingDamages.Remove(damage);
			}
		}

		public void DealPermanentDamage(IDamagableView damageTaker, int damage, IUnit owner) {
			damageTaker.TakeDamage(damage, owner);
			if (damageTaker is PlayerView enemyPlayer && enemyPlayer != owner.UnitView) {
				OnDamageEnemyPlayer?.Invoke(new PhotonDamageInfo(enemyPlayer.PhotonView.ViewID, damage));
			}
		}

		public void DealPeriodicalDamage(IDamagableView damageTaker, int[] damages, float damageTicksCooldown, IUnit damager) {
			float currentCooldown = 0;
			foreach (var damage in damages) {
				_comingDamages.Add(new ComingDamage(currentCooldown, damageTaker, damage, damager));
				currentCooldown += damageTicksCooldown;
			}
		}

		public void StopPeriodicalDamageForUnit(IUnit unit) {
			var damageTaker = unit.UnitView as IDamagableView;
			foreach (var comingDamage in _comingDamages.Where(damage => damage.DamageTaker == damageTaker)) {
				comingDamage.StopDamage();
			}
		}

	}

}