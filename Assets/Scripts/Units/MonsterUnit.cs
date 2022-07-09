using Infrastructure;
using Services;
using Units.Views;
using UnityEngine;


namespace Units {

	internal class MonsterUnit : Unit {
		private readonly int _collisionDamage;
		private readonly IUnitsPool _unitsPool;
		private readonly int _killExperience;
		private MonsterView MonsterView => _unitView as MonsterView;
		
		
		public MonsterUnit(GameObject playerGO, MonstersParams monstersParam, IUnitsPool unitsPool) : base(monstersParam.MoveSpeed, monstersParam.Hp) {
			Experience = new Experience(monstersParam.MonsterLevel, null);
			_unitsPool = unitsPool;
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

		private void TakeDamage(int damage) {
			Health.TakeDamage(damage);
		}

		private void DamageTriggerUnit(Collision2D otherUnit) {
			if (otherUnit.gameObject.TryGetComponent<IDamagableView>(out var damageTaker)) {
				if (damageTaker is MonsterView)
					return;
				
				damageTaker.TakeDamage(_collisionDamage);
				KillMonster();
			}
			if (IsDead && otherUnit.gameObject.TryGetComponent<IExperienceReciever>(out var experienceReciever)) {
				experienceReciever.RecieveExperience(_killExperience);
			}
		}

		protected override void DestroyView() {
			_unitsPool.ReturnObject(this);
		}
	}

}