using System;
using Services;
using Units;
using UnityEngine;


namespace Infrastructure {

	internal interface IAmmo : IPoolObject, IDisposable {
		void Init(IUnit owner, int[] damage, float damageTicksCooldown, bool isPiercing);
		void Push(Vector3 power);
	}

}