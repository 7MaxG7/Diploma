using Infrastructure;
using Units.Views;
using UnityEngine;


namespace Units {

	internal class MonsterUnit : Unit {
		private readonly int _collisionDamage;

		public MonsterUnit(GameObject playerGO, MonstersParams monstersParam) : base(playerGO, monstersParam.MoveSpeed, monstersParam.Hp) {
			_collisionDamage = monstersParam.Damage;
			var monsterView = playerGO.AddComponent<MonsterView>();
			monsterView.OnCollisionEnter += DamageCollisionUnit;
			monsterView.OnDamageTake += TakeDamage;
			_unitView = monsterView;
		}

		private void TakeDamage(int damage) {
			Health.TakeDamage(damage);
		}

		private void DamageCollisionUnit(ControllerColliderHit otherUnit) {
			if (otherUnit.gameObject.TryGetComponent<IDamagableView>(out var damageTaker)) {
				if (damageTaker is MonsterView)
					return;
				
				damageTaker.TakeDamage(_collisionDamage);
				KillMonster();
			}
		}

	}

}