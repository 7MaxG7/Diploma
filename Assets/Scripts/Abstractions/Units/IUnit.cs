using Infrastructure;
using UnityEngine;


namespace Units {

	internal interface IUnit : IPoolObject {
		CharacterController CharacterController { get; }
		float MoveSpeed { get; }
		bool IsDead { get; }
	}

}