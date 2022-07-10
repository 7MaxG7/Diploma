using System;
using System.Collections.Generic;
using UnityEngine;


namespace Services {

	internal abstract class ObjectsPool<T> where T : IPoolObject {
		public event Action<int> OnObjectInstantiated;
		public event Action<int, bool> OnObjectActivationToggle;
		
		private readonly IPhotonDataExchangeController _photonDataExchangeController;
		protected readonly List<T> _objects = new();
		protected readonly List<T> _spawnedObjects = new();


		public T SpawnObject(Vector2 spawnPosition, params object[] parameters) {
			T obj;
			if (_objects.Count == 0) {
				_objects.Capacity++;
				obj = SpawnSpecifiedObject(spawnPosition, parameters);
				OnObjectInstantiated?.Invoke(obj.PhotonView.ViewID);
			} else {
				var objIndex = _objects.Count - 1;
				obj = _objects[objIndex];
				_objects.RemoveAt(objIndex);
				obj.Respawn(spawnPosition);
			}
			_spawnedObjects.Add(obj);
			TogglePoolObjectActivation(obj, true);
			return obj;
		}

		public void ReturnObject(T obj) {
			TogglePoolObjectActivation(obj, false);
			obj.StopObj();
			obj.Transform.position = Vector3.zero;
			_objects.Add(obj);
			_spawnedObjects.Remove(obj);
		}

		protected abstract T SpawnSpecifiedObject(Vector2 spawnPosition, object[] parameters);

		private void TogglePoolObjectActivation(T obj, bool isActive) {
			obj.GameObject.SetActive(isActive);
			OnObjectActivationToggle?.Invoke(obj.PhotonView.ViewID, isActive);
		}

	}

}