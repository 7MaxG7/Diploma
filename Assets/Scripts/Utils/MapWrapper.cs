using System.Collections.Generic;
using UnityEngine;


namespace Infrastructure {

	internal class MapWrapper : IMapWrapper {

		private float _bottomMapSidePosition;
		private float _topMapSidePosition;
		private float _leftMapSidePosition;
		private float _rightMapSidePosition;

		public void Init(Vector2 bottomLeftCornerPosition, Vector2 topRightCornerPosition) {
			_bottomMapSidePosition = bottomLeftCornerPosition.y;
			_topMapSidePosition = topRightCornerPosition.y;
			_leftMapSidePosition = bottomLeftCornerPosition.x;
			_rightMapSidePosition = topRightCornerPosition.x;
		}

		public void CheckAndReturnInsideMap(Transform checkingTransform, IEnumerable<Transform> dependingTransforms) {
			if (!ChangingPositionIsReqired(checkingTransform.position)) {
				return;
			}

			var deltaPosition = Vector2.zero;
			var mapWidth = _rightMapSidePosition - _leftMapSidePosition;
			if (checkingTransform.position.x < _leftMapSidePosition)
				deltaPosition.x = mapWidth;
			else if (checkingTransform.position.x > _rightMapSidePosition)
				deltaPosition.x = -mapWidth;

			var mapHeight = _topMapSidePosition - _bottomMapSidePosition;
			if (checkingTransform.position.y < _bottomMapSidePosition)
				deltaPosition.y = mapHeight;
			else if (checkingTransform.position.y > _topMapSidePosition)
				deltaPosition.y = -mapHeight;

			ChangePosition(checkingTransform, deltaPosition);
			foreach (var dependingTransform in dependingTransforms) {
				ChangePosition(dependingTransform, deltaPosition);
			}
		}

		private bool ChangingPositionIsReqired(Vector2 position) {
			return position.x < _leftMapSidePosition
			       || position.y < _bottomMapSidePosition
			       || position.x > _rightMapSidePosition
			       || position.y > _topMapSidePosition;
		}

		private void ChangePosition(Transform objTransform, Vector2 deltaPosition) {
			Vector2 position = objTransform.position;
			position += deltaPosition;
			objTransform.position = position;
		}
	}

}