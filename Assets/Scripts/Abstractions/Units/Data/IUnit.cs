using System;
using Services;
using Units.Views;
using UnityEngine;


namespace Units {

	internal interface IUnit : IPoolObject, IDisposable {
		event Action<IUnit> OnDied;
		
		float MoveSpeed { get; }
		bool IsDead { get; }
		Health Health { get; }
		Experience Experience { get; }
		Rigidbody2D Rigidbody { get; }
		UnitView UnitView { get; }

		bool CheckOwnView(IDamagableView damageTaker);

		void KillUnit();
	}

}