using UnityEngine;


namespace Infrastructure {

	internal interface IMapWrapper {
		void Init(Vector2 bottomLeftCornerPosition, Vector2 topRightCornerPosition);
		void CheckAndReturnInsideMap(Transform checkingTransform);
	}

}