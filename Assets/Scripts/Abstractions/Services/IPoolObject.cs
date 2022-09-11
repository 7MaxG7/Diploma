using Photon.Pun;
using UnityEngine;


namespace Services {

	internal interface IPoolObject : IMovable, IView {
		PhotonView PhotonView { get; }
		int PoolIndex { get; }

		void Respawn(Vector2 spawnPosition);
		void ToggleActivation(bool isActive);
	}

}