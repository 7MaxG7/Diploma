using Infrastructure;
using Services;
using Units.Views;
using UnityEngine;


namespace Units {

	internal class MonsterUnit : Unit {
		private readonly int _collisionDamage;
		private readonly IUnitsPool _unitsPool;
		private readonly IHandleDamageController _handleDamageController;
		private readonly int _killExperience;
		private MonsterView MonsterView => _unitView as MonsterView;
		
		
		public MonsterUnit(GameObject playerGO, MonstersParams monstersParam, IUnitsPool unitsPool, IHandleDamageController handleDamageController) 
				: base(monstersParam.MoveSpeed, monstersParam.Hp) {
			Experience = new Experience(monstersParam.MonsterLevel, null);
			_unitsPool = unitsPool;
			_handleDamageController = handleDamageController;
			_collisionDamage = monstersParam.Damage;
			_killExperience = monstersParam.ExperienceOnKill;
			var monsterView = playerGO.GetComponent<MonsterView>();
			monsterView.OnTriggerEnter += DamageTriggerUnit;
			monsterView.OnDamageTake += TakeDamage;
			_unitView = monsterView;
		}

		public override void Dispose() {
			base.Dispose();
			_unitView.OnDamageTake -= TakeDamage;
			MonsterView.OnTriggerEnter -= DamageTriggerUnit;
		}

		private void TakeDamage(int damage, IUnit damager) {
			Health.TakeDamage(damage);
			if (IsDead && damager is IExperienceReciever experienceReciever)
				experienceReciever.RecieveExperience(_killExperience);
		}

		private void DamageTriggerUnit(Collision2D otherUnit) {
			if (otherUnit.gameObject.TryGetComponent<IDamagableView>(out var damageTaker)) {
				if (damageTaker is MonsterView)
					return;
				
				damageTaker.TakeDamage(_collisionDamage, this);
				KillMonster();
			}
		}

		protected override void DestroyView() {
			_handleDamageController.StopPeriodicalDamageForUnit(MonsterView);
			_unitsPool.ReturnObject(this);
		}
	}

}