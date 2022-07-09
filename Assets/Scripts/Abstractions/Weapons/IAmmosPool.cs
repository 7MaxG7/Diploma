using UnityEngine;


namespace Infrastructure {

	internal interface IAmmosPool {
		IAmmo SpawnObject(Vector2 spawnPosition, params object[] parameters);
		void ReturnObject(IAmmo obj);
	}

}