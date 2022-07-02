using System.Collections.Generic;
using Enums;
using UnityEngine;


namespace Services {

	internal abstract class ObjectsPool<T> where T : IPoolObject {
		private PhotonDataExchanger _photonDataExchanger;
		private readonly List<T> _objects = new();


		public void Init(PhotonDataExchanger photonDataExchanger) {
			_photonDataExchanger = photonDataExchanger;
		}

		public T SpawnObject(Vector2 spawnPosition, params object[] parameters) {
			T obj;
			if (_objects.Count == 0) {
				_objects.Capacity++;
				obj = SpawnSpecifiedObject(spawnPosition, parameters);
				_photonDataExchanger.SendData(PhotonSynchronizerDataType.ObjectInstantiation, obj.PhotonView.ViewID);
			} else {
				var objIndex = _objects.Count - 1;
				obj = _objects[objIndex];
				_objects.RemoveAt(objIndex);
				obj.Respawn(spawnPosition);
			}
			TogglePoolObjectActivation(obj, true);
			return obj;
		}

		public void ReturnObject(T obj) {
			TogglePoolObjectActivation(obj, false);
			obj.Transform.position = Vector3.zero;
			_objects.Add(obj);
		}

		protected abstract T SpawnSpecifiedObject(Vector2 spawnPosition, object[] parameters);

		private void TogglePoolObjectActivation(T obj, bool isActive) {
			obj.GameObject.SetActive(isActive);
			_photonDataExchanger.SendData(PhotonSynchronizerDataType.ObjectActivation, obj.PhotonView.ViewID, isActive);
		}

	}

}