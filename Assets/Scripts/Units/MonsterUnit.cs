using Infrastructure;
using Photon.Pun;
using Units.Views;
using UnityEngine;


namespace Units {

	internal class MonsterUnit : Unit {
		private readonly int _collisionDamage;
		private readonly int _killExperience;
		private MonsterView MonsterView => UnitView as MonsterView;
		
		
		public MonsterUnit(GameObject monsterGO, MonstersParams monstersParam, int poolIndex) : base(monstersParam.MoveSpeed, monstersParam.Hp, poolIndex) {
			Experience = new Experience(monstersParam.MonsterLevel, null);
			_collisionDamage = monstersParam.Damage;
			_killExperience = monstersParam.ExperienceOnKill;
			var monsterView = monsterGO.GetComponent<MonsterView>();
			monsterView.OnTriggerEnter += DamageTriggerUnit;
			monsterView.OnDamageTake += TakeDamage;
			UnitView = monsterView;
		}

		public override void Dispose() {
			MonsterView.OnDamageTake -= TakeDamage;
			MonsterView.OnTriggerEnter -= DamageTriggerUnit;
			PhotonNetwork.Destroy(UnitView.GameObject);
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