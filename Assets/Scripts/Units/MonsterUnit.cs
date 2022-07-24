using Infrastructure;
using Units.Views;
using UnityEngine;


namespace Units {

	internal class MonsterUnit : Unit {
		private readonly int _collisionDamage;
		private readonly int _killExperience;
		private MonsterView MonsterView => UnitView as MonsterView;
		
		
		public MonsterUnit(GameObject playerGO, MonstersParams monstersParam, int poolIndex) : base(monstersParam.MoveSpeed, monstersParam.Hp, poolIndex) {
			Experience = new Experience(monstersParam.MonsterLevel, null);
			_collisionDamage = monstersParam.Damage;
			_killExperience = monstersParam.ExperienceOnKill;
			var monsterView = playerGO.GetComponent<MonsterView>();
			monsterView.OnTriggerEnter += DamageTriggerUnit;
			monsterView.OnDamageTake += TakeDamage;
			UnitView = monsterView;
		}

		public override void Dispose() {
			base.Dispose();
			UnitView.OnDamageTake -= TakeDamage;
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
				KillUnit();
			}
		}
	}

}