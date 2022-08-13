using System;
using UnityEngine;


namespace Infrastructure {

	internal interface IAmmosPool : IDisposable {
		event Action<int> OnObjectInstantiated;
		event Action<int, bool> OnObjectActivationToggle;
		
		IAmmo SpawnObject(Vector2 spawnPosition, params object[] parameters);
		void ReturnObject(IAmmo obj);
	}

}