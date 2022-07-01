using UnityEngine;


namespace Infrastructure {

	internal interface IPoolObject {
		GameObject GameObject { get; }
		Transform Transform { get; }

		void Respawn(Vector2 spawnPosition);
	}

}