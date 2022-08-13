using System;
using Services;
using Units;


namespace Infrastructure {

	internal interface IHandleDamageController : IUpdater {
		event Action<PhotonDamageInfo> OnDamageEnemyPlayer;
		
		void DealPermanentDamage(IDamagableView damageTaker, int damage, IUnit owner);
		void DealPeriodicalDamage(IDamagableView damageTaker, int[] damages, float damageTicksCooldown, IUnit damager);
		void StopPeriodicalDamageForUnit(IUnit unit);
	}

}