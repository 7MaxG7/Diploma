﻿using Units;
using Units.Views;


namespace Infrastructure {

	internal interface IHandleDamageController : IUpdater {
		void DealPermanentDamage(IDamagableView damageTaker, int damage, IUnit owner);
		void DealPeriodicalDamage(IDamagableView damageTaker, int[] damages, float damageTicksCooldown, IUnit damager);
		void StopPeriodicalDamageForUnit(IDamagableView damageTaker);
	}

}