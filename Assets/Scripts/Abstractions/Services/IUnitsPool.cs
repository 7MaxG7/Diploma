using System;
using Units;
using UnityEngine;


namespace Services {

	internal interface IUnitsPool {
		event Action<int> OnObjectInstantiated;
		event Action<int, bool> OnObjectActivationToggle;
		IUnit SpawnObject(Vector2 spawnPosition, params object[] parameters);
		void ReturnObject(IUnit obj);
	}

}