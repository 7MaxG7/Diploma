using Units;
using UnityEngine;


namespace Infrastructure {

	internal interface IUnitsPool {
		IUnit SpawnObject(Vector2 spawnPosition, params object[] parameters);
		void ReturnObject(IUnit obj);
	}

}