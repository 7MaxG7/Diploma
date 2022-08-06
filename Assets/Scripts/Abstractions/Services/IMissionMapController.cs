using UnityEngine;


namespace Infrastructure {

	internal interface IMissionMapController : IUpdater {
		Transform[] GroundItems { get; }
		
		void Init(Transform playerTransform, Vector2 groundItemSize);
	}

}