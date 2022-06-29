using UnityEngine;


namespace Infrastructure {

	internal interface IMissionMapController : IUpdater {
		void Init(Transform playerTransform, Vector2 groundItemSize);
	}

}