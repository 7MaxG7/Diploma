using System;
using Services;
using UnityEngine;


namespace Units {

	internal interface IUnit : IPoolObject, IDisposable {
		event Action<DamageInfo> OnDied;
		
		float MoveSpeed { get; }
		bool IsDead { get; }
		Health Health { get; }
		Experience Experience { get; }
		UnitView UnitView { get; }

		bool CheckOwnView(IDamagableView damageTaker);
		void KillUnit();
		void Move(Vector3 deltaPosition);
	}

}