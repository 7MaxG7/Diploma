using System;
using Services;
using Units;
using UnityEngine;


namespace Infrastructure {

	internal interface IAmmo : IPoolObject, IDisposable {
		Rigidbody2D RigidBody { get; }

		void Init(IUnit owner, int[] damage, float damageTicksCooldown, bool isPiercing);
	}

}