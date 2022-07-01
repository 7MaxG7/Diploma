using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Infrastructure {

	internal abstract class ObjectsPool<T> where T : IPoolObject {
		private readonly List<T> _objects = new();

		public T SpawnObject(Vector2 spawnPosition, params object[] parameters) {
			T obj;
			if (_objects.Count == 0) {
				_objects.Capacity++;
				obj = SpawnSpecifiedObject(spawnPosition, parameters);
			} else {
				var objIndex = _objects.Count - 1;
				obj = _objects[objIndex];
				_objects.RemoveAt(objIndex);
				obj.Respawn(spawnPosition);
			}
			return obj;
		}

		public void ReturnObject(T obj) {
			obj.GameObject.SetActive(false);
			obj.Transform.position = Vector3.zero;
			_objects.Add(obj);
		}

		protected abstract T SpawnSpecifiedObject(Vector2 spawnPosition, object[] parameters);
	}

}