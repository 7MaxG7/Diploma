using UnityEngine;


namespace Units {

	internal interface IUnit {
		Transform Transform { get; }
		CharacterController CharacterController { get; }
		float MoveSpeed { get; }
		bool IsDead { get; }
	}

}