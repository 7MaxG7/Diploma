using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


namespace Services {

	internal sealed class MissionMapManager : IMissionMapManager {
		private readonly IViewsFactory _viewsFactory;
		private readonly MissionConfig _missionConfig;
		private Transform _playerTransform;
		private Vector2 _groundItemSize;
		private GroundItemView[] _groundItems;
		

		[Inject]
		public MissionMapManager(IViewsFactory viewsFactory, IControllersHolder controllersHolder, MissionConfig missionConfig) {
			controllersHolder.AddController(this);
			_viewsFactory = viewsFactory;
			_missionConfig = missionConfig;
		}

		public void Dispose() {
			foreach (var groundItem in _groundItems) {
				_viewsFactory.DestroyView(groundItem);
			}
			_groundItems = null;
			_playerTransform = null;
		}

		public void OnUpdate(float deltaTime) {
			if (_groundItems == null || _playerTransform == null)
				return;
			
			foreach (var groundItem in _groundItems) {
				if (ItemIsTooLeft(groundItem.Transform.position.x)) {
					groundItem.RelocateBy(new Vector3(_groundItemSize.x * Constants.GROUND_ITEM_RELOCATION_DISTANCE_RATE, 0, 0));
				} else if (ItemIsTooRight(groundItem.Transform.position.x)) {
					groundItem.RelocateBy(new Vector3(- _groundItemSize.x * Constants.GROUND_ITEM_RELOCATION_DISTANCE_RATE, 0, 0));
				}
				if (ItemIsTooLow(groundItem.Transform.position.y)) {
					groundItem.RelocateBy(new Vector3(0, _groundItemSize.y * Constants.GROUND_ITEM_RELOCATION_DISTANCE_RATE, 0));
				} else if (ItemIsTooHigh(groundItem.Transform.position.y)) {
					groundItem.RelocateBy(new Vector3(0, - _groundItemSize.y * Constants.GROUND_ITEM_RELOCATION_DISTANCE_RATE, 0));
				}
			}
			
			bool ItemIsTooLeft(float itemPositionX) {
				return ItemIsTooFar(_playerTransform.position.x, itemPositionX, _groundItemSize.x * Constants.MAX_PLAYER_TO_GROUND_ITEM_DISTANCE_RATE);
			}

			bool ItemIsTooRight(float itemPositionX) {
				return ItemIsTooFar(itemPositionX, _playerTransform.position.x, _groundItemSize.x * Constants.MAX_PLAYER_TO_GROUND_ITEM_DISTANCE_RATE);
			}

			bool ItemIsTooLow(float itemPositionY) {
				return ItemIsTooFar(_playerTransform.position.y, itemPositionY, _groundItemSize.y * Constants.MAX_PLAYER_TO_GROUND_ITEM_DISTANCE_RATE);
			}

			bool ItemIsTooHigh(float itemPositionY) {
				return ItemIsTooFar(itemPositionY, _playerTransform.position.y, _groundItemSize.y * Constants.MAX_PLAYER_TO_GROUND_ITEM_DISTANCE_RATE);
			}

			bool ItemIsTooFar(float higherValuePosition, float lowerValuePosition, float maxDistance) {
				return higherValuePosition - lowerValuePosition > maxDistance;
			}
		}

		public void Init(Transform playerTransform, Vector2 groundItemSize, out IEnumerable<IView> groundItems) {
			InitFields();
			InstantiateGround();
			groundItems = _groundItems;
			
			void InitFields() {
				_playerTransform = playerTransform;
				_groundItemSize = groundItemSize;
			}

			void InstantiateGround() {
				_groundItems = new GroundItemView[Constants.HORIZONTAL_GROUND_ITEMS_AMOUNT * Constants.VERTICAL_GROUND_ITEMS_AMOUNT];
				var groundParent = _viewsFactory.CreateGameObject(Constants.GROUND_ITEMS_PARENT_NAME).transform; 
				for (var i = 0; i < Constants.HORIZONTAL_GROUND_ITEMS_AMOUNT; i++) {
					for (var j = 0; j < Constants.VERTICAL_GROUND_ITEMS_AMOUNT; j++) {
						var groundItem = Object.Instantiate(_missionConfig.GroundItemPref, groundParent);
						var groundPosition = playerTransform.position + new Vector3(_groundItemSize.x * (i - 1), _groundItemSize.y * (j - 1));
						groundItem.RelocateBy(groundPosition);
						_groundItems[i * Constants.VERTICAL_GROUND_ITEMS_AMOUNT + j] = groundItem;
					}
				}
			}
		}

	}

}