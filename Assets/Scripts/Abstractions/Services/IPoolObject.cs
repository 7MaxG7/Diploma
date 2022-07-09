﻿using Photon.Pun;
using UnityEngine;


namespace Services {

	internal interface IPoolObject : IMovable {
		GameObject GameObject { get; }
		Transform Transform { get; }
		PhotonView PhotonView { get; }

		void Respawn(Vector2 spawnPosition);
	}

}