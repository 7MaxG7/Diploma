using System;
using Services;
using Units;
using UnityEngine;


namespace Weapons {

	internal interface IAmmo : IPoolObject, IDisposable {
		event Action<IAmmo> OnLifetimeExpired;
		event Action<IDamagableView, int[], float, IUnit> OnCollidedWithDamageTaker;

		void Init(IUnit owner, int[] damage, float damageTicksCooldown, bool isPiercing);
		void Push(Vector3 power);
	}

}