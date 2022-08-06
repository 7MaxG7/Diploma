using System;
using System.Collections.Generic;
using UnityEngine;


namespace Services {

	internal abstract class ObjectsPool<T> where T : IPoolObject {
		public event Action<int> OnObjectInstantiated;
		public event Action<int, bool> OnObjectActivationToggle;
		
		private readonly IPhotonDataExchangeController _photonDataExchangeController;
		// ReSharper disable once InconsistentNaming
		protected readonly Dictionary<int,List<T>> _objects = new();
		// ReSharper disable once InconsistentNaming
		protected readonly List<T> _spawnedObjects = new();


		public T SpawnObject(Vector2 spawnPosition, params object[] parameters) {
			T obj;
			var poolIndex = GetSpecifiedPoolIndex(parameters);
			if (!_objects.ContainsKey(poolIndex))
				_objects.Add(poolIndex, new List<T>());
			
			var objects = _objects[poolIndex];
			if (objects.Count == 0) {
				objects.Capacity++;
				obj = SpawnSpecifiedObject(spawnPosition, parameters);
				OnObjectInstantiated?.Invoke(obj.PhotonView.ViewID);
			} else {
				var objIndex = objects.Count - 1;
				obj = objects[objIndex];
				objects.RemoveAt(objIndex);
				obj.Respawn(spawnPosition);
			}
			_spawnedObjects.Add(obj);
			TogglePoolObjectActivation(obj, true);
			return obj;
		}

		public void ReturnObject(T obj) {
			// In case of ammo it can be returned twice: on collision it returnes, deactivates and becomes invisible - so returnes second time OnBecameInvisible.
			// Thats why we check if its active first
			if (!_spawnedObjects.Remove(obj))
				return;

			TogglePoolObjectActivation(obj, false);
			obj.StopObj();
			obj.Transform.position = Vector3.zero;
			_objects[obj.PoolIndex].Add(obj);
		}

		private void TogglePoolObjectActivation(T obj, bool isActive) {
			obj.GameObject.SetActive(isActive);
			OnObjectActivationToggle?.Invoke(obj.PhotonView.ViewID, isActive);
		}

		protected abstract int GetSpecifiedPoolIndex(object[] parameters);

		protected abstract T SpawnSpecifiedObject(Vector2 spawnPosition, object[] parameters);
	}

}