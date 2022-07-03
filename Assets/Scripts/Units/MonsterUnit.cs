using Infrastructure;
using Services;
using Units.Views;
using UnityEngine;


namespace Units {

	internal class MonsterUnit : Unit {
		private readonly int _collisionDamage;
		private readonly IUnitsPool _unitsPool;
		private MonsterView MonsterView => _unitView as MonsterView;
		
		
		public MonsterUnit(GameObject playerGO, MonstersParams monstersParam, IUnitsPool unitsPool) : base(monstersParam.MoveSpeed, monstersParam.Hp) {
			_unitsPool = unitsPool;
			_collisionDamage = monstersParam.Damage;
			var monsterView = playerGO.GetComponent<MonsterView>();
			monsterView.OnCollisionEnter += DamageCollisionUnit;
			monsterView.OnDamageTake += TakeDamage;
			_unitView = monsterView;
		}

		public override void Dispose() {
			base.Dispose();
			_unitView.OnDamageTake -= TakeDamage;
			MonsterView.OnCollisionEnter -= DamageCollisionUnit;
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

		protected override void DestroyView() {
			_unitsPool.ReturnObject(this);
		}
	}

}