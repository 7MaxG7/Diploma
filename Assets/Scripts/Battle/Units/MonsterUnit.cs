using Photon.Pun;
using UnityEngine;


namespace Units {

	internal sealed class MonsterUnit : Unit {
		public override IUnitView UnitView => _monsterView;
		public override Transform Transform => _monsterView.Transform;
		public override PhotonView PhotonView => _monsterView.PhotonView;

		private readonly MonsterView _monsterView;
		private readonly int _collisionDamage;
		private readonly int _killExperience;


		public MonsterUnit(GameObject monsterGo, MonstersParams monstersParam, int poolIndex) 
				: base(monstersParam.MoveSpeed, monstersParam.Hp, poolIndex) {
			Experience = new Experience(monstersParam.MonsterLevel, null);
			_collisionDamage = monstersParam.Damage;
			_killExperience = monstersParam.ExperienceOnKill;
			var monsterView = monsterGo.GetComponent<MonsterView>();
			monsterView.OnTriggerEnter += DamageTriggerUnit;
			monsterView.OnDamageTake += TakeDamage;
			_monsterView = monsterView;
		}

		public override void Dispose() {
			_monsterView.OnDamageTake -= TakeDamage;
			_monsterView.OnTriggerEnter -= DamageTriggerUnit;
			base.Dispose();
		}

		private void TakeDamage(int damage, IUnit damager) {
			var damageInfo = new DamageInfo(damage, damager, this);
			Health.TakeDamage(damageInfo);
			if (IsDead && damager is IExperienceReciever experienceReciever)
				experienceReciever.RecieveExperience(_killExperience);
		}

		private void DamageTriggerUnit(Collision2D otherUnit) {
			if (otherUnit.gameObject.TryGetComponent<IDamagableView>(out var damageTaker)) {
				if (damageTaker is MonsterView)
					return;
				
				KillUnit();
				damageTaker.TakeDamage(_collisionDamage, this);
			}
		}
	}

}