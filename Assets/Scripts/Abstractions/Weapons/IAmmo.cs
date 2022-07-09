using Services;
using Units;
using UnityEngine;


namespace Infrastructure {

	internal interface IAmmo : IPoolObject {
		Rigidbody2D RigidBody { get; }

		void Init(IUnit owner);
	}

}