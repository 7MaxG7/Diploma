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

		public void CheckAndReturnInsideMap(Transform checkingTransform) {
			var position = checkingTransform.position;
			if (!ChangingPositionIsReqired(position)) {
				return;
			}

			var mapWidth = _rightMapSidePosition - _leftMapSidePosition;
			if (position.x < _leftMapSidePosition)
				position.x += mapWidth;
			else if (position.x > _rightMapSidePosition)
				position.x -= mapWidth;

			var mapHeight = _topMapSidePosition - _bottomMapSidePosition;
			if (position.y < _bottomMapSidePosition)
				position.y += mapHeight;
			else if (position.y > _topMapSidePosition)
				position.y -= mapHeight;

			checkingTransform.position = position;
		}

		private bool ChangingPositionIsReqired(Vector2 position) {
			return position.x < _leftMapSidePosition
			       || position.y < _bottomMapSidePosition
			       || position.x > _rightMapSidePosition
			       || position.y > _topMapSidePosition;
		}
	}

}