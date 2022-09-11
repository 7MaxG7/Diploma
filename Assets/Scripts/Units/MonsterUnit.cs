using Infrastructure;
using Photon.Pun;
using Services;
using Units.Views;
using UnityEngine;


namespace Units {

	internal sealed class MonsterUnit : Unit {
		public override UnitView UnitView => _monsterView;
		public override Transform Transform => _monsterView.Transform;
		public override PhotonView PhotonView => _monsterView.PhotonView;

		private readonly MonsterView _monsterView;
		private readonly IViewsFactory _viewsFactory;
		private readonly int _collisionDamage;
		private readonly int _killExperience;


		public MonsterUnit(GameObject monsterGO, MonstersParams monstersParam, int poolIndex, IViewsFactory viewsFactory) 
				: base(monstersParam.MoveSpeed, monstersParam.Hp, poolIndex) {
			_viewsFactory = viewsFactory;
			Experience = new Experience(monstersParam.MonsterLevel, null);
			_collisionDamage = monstersParam.Damage;
			_killExperience = monstersParam.ExperienceOnKill;
			var monsterView = monsterGO.GetComponent<MonsterView>();
			monsterView.OnTriggerEnter += DamageTriggerUnit;
			monsterView.OnDamageTake += TakeDamage;
			_monsterView = monsterView;
		}

		public override void Dispose() {
			_monsterView.OnDamageTake -= TakeDamage;
			_monsterView.OnTriggerEnter -= DamageTriggerUnit;
			_viewsFactory.DestroyPhotonObj(PhotonView);
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