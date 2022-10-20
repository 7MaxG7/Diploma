using System.Collections.Generic;
using System.Linq;
using Services;
using UnityEngine;
using Zenject;


namespace Infrastructure
{
    internal sealed class MapWrapper : IMapWrapper
    {
        private Transform _checkingTransform;
        private readonly List<IEnumerable<IView>> _dependingTransforms = new();
        private float _bottomMapSidePosition;
        private float _topMapSidePosition;
        private float _leftMapSidePosition;
        private float _rightMapSidePosition;


        [Inject]
        public MapWrapper(IControllersHolder controllersHolder)
        {
            controllersHolder.AddController(this);
        }

        public void Dispose()
        {
            _checkingTransform = null;
            _dependingTransforms.Clear();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_checkingTransform == null)
                return;

            CheckAndReturnInsideMap();
        }

        public void Init(Vector2 bottomLeftCornerPosition, Vector2 topRightCornerPosition)
        {
            _bottomMapSidePosition = bottomLeftCornerPosition.y;
            _topMapSidePosition = topRightCornerPosition.y;
            _leftMapSidePosition = bottomLeftCornerPosition.x;
            _rightMapSidePosition = topRightCornerPosition.x;
        }

        public void SetCheckingTransform(Transform checkingTransform)
        {
            _checkingTransform = checkingTransform;
        }

        public void AddDependingTransforms(IEnumerable<IView> dependingTransforms)
        {
            _dependingTransforms.Add(dependingTransforms);
        }

        private void CheckAndReturnInsideMap()
        {
            if (!ChangingPositionIsRequired(_checkingTransform.position))
            {
                return;
            }

            var deltaPosition = Vector2.zero;
            var mapWidth = _rightMapSidePosition - _leftMapSidePosition;
            if (_checkingTransform.position.x < _leftMapSidePosition)
                deltaPosition.x = mapWidth;
            else if (_checkingTransform.position.x > _rightMapSidePosition)
                deltaPosition.x = -mapWidth;

            var mapHeight = _topMapSidePosition - _bottomMapSidePosition;
            if (_checkingTransform.position.y < _bottomMapSidePosition)
                deltaPosition.y = mapHeight;
            else if (_checkingTransform.position.y > _topMapSidePosition)
                deltaPosition.y = -mapHeight;

            ChangePosition(_checkingTransform, deltaPosition);
            foreach (var dependingTransform in _dependingTransforms.SelectMany(items =>
                         items.Select(item => item.Transform)))
            {
                ChangePosition(dependingTransform, deltaPosition);
            }
        }

        private bool ChangingPositionIsRequired(Vector2 position)
        {
            return position.x < _leftMapSidePosition
                   || position.y < _bottomMapSidePosition
                   || position.x > _rightMapSidePosition
                   || position.y > _topMapSidePosition;
        }

        private void ChangePosition(Transform objTransform, Vector2 deltaPosition)
        {
            Vector2 position = objTransform.position;
            position += deltaPosition;
            objTransform.position = position;
        }
    }
}