using System;
using UnityEngine;


namespace Infrastructure {

	internal interface IMissionMapController : IUpdater, IDisposable {
		Transform[] GroundItems { get; }
		
		void Init(Transform playerTransform, Vector2 groundItemSize);
	}

}