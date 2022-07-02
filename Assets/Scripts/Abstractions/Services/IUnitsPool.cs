using Units;
using UnityEngine;


namespace Services {

	internal interface IUnitsPool {
		void Init(PhotonDataExchanger photonDataExchanger);
		IUnit SpawnObject(Vector2 spawnPosition, params object[] parameters);
		void ReturnObject(IUnit obj);
	}

}