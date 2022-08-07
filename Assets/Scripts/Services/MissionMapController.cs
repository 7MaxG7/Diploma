using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal class MissionMapController : IMissionMapController {
		private const int HORIZONTAL_GROUND_ITEMS_AMOUNT = 3;
		private const int VERTICAL_GROUND_ITEMS_AMOUNT = 3;
		/// <summary>
		/// На какую дистанцию игрок должен отдалиться от позиции объекта, чтобы тот релоцировался (в размерах объекта)
		/// </summary>
		private const float MAX_PLAYER_TO_GROUND_ITEM_DISTANCE_RATE = 1.5f;
		/// <summary>
		/// На какую дистанцию релоцируется объект, если игрок достаточно от него отдалился (в размерах объекта)
		/// </summary>
		private const int GROUND_ITEM_RELOCATION_DISTANCE_RATE = 3;

		public Transform[] GroundItems { get; private set; }

		private readonly MissionConfig _missionConfig;
		private Transform _playerTransform;
		private Vector2 _groundItemSize;
		
		private Vector3 _groundItemPositionTmp;


		[Inject]
		public MissionMapController(IControllersHolder controllersHolder, MissionConfig missionConfig) {
			controllersHolder.AddController(this);
			_missionConfig = missionConfig;
		}

		public void Dispose() {
			_playerTransform = null;
			GroundItems = null;
		}
		
		public void Init(Transform playerTransform, Vector2 groundItemSize) {
			InitFields();
			InstantiateGround();

			
			void InitFields() {
				_playerTransform = playerTransform;
				_groundItemSize = groundItemSize;
			}

			void InstantiateGround() {
				GroundItems = new Transform[HORIZONTAL_GROUND_ITEMS_AMOUNT * VERTICAL_GROUND_ITEMS_AMOUNT];
				var groundParent = new GameObject(TextConstants.GROUND_ITEMS_PARENT_NAME).transform;
				for (var i = 0; i < HORIZONTAL_GROUND_ITEMS_AMOUNT; i++) {
					for (var j = 0; j < VERTICAL_GROUND_ITEMS_AMOUNT; j++) {
						var groundPosition = playerTransform.position + new Vector3(_groundItemSize.x * (i - 1), _groundItemSize.y * (j - 1));
						var groundItem = Object.Instantiate(_missionConfig.GroundItemPref, groundPosition, Quaternion.identity, groundParent);
						GroundItems[i * VERTICAL_GROUND_ITEMS_AMOUNT + j] = groundItem;
					}
				}
			}
		}

		public void OnUpdate(float deltaTime) {
			if (GroundItems == null || _playerTransform == null)
				return;
			
			RelocateGround();

			
			void RelocateGround() {
				foreach (var groundItem in GroundItems) {
					_groundItemPositionTmp = groundItem.position;
					if (ItemIsTooLeft()) {
						_groundItemPositionTmp.x += _groundItemSize.x * GROUND_ITEM_RELOCATION_DISTANCE_RATE;
					} else if (ItemIsTooRight()) {
						_groundItemPositionTmp.x -= _groundItemSize.x * GROUND_ITEM_RELOCATION_DISTANCE_RATE;
					}
					if (ItemIsTooLow()) {
						_groundItemPositionTmp.y += _groundItemSize.y * GROUND_ITEM_RELOCATION_DISTANCE_RATE;
					} else if (ItemIsTooHigh()) {
						_groundItemPositionTmp.y -= _groundItemSize.y * GROUND_ITEM_RELOCATION_DISTANCE_RATE;
					}
					groundItem.position = _groundItemPositionTmp;
				}
			}

			
			bool ItemIsTooLeft() {
				return ItemIsTooFar(_playerTransform.position.x, _groundItemPositionTmp.x, _groundItemSize.x * MAX_PLAYER_TO_GROUND_ITEM_DISTANCE_RATE);
			}

			bool ItemIsTooRight() {
				return ItemIsTooFar(_groundItemPositionTmp.x, _playerTransform.position.x, _groundItemSize.x * MAX_PLAYER_TO_GROUND_ITEM_DISTANCE_RATE);
			}

			bool ItemIsTooLow() {
				return ItemIsTooFar(_playerTransform.position.y, _groundItemPositionTmp.y, _groundItemSize.y * MAX_PLAYER_TO_GROUND_ITEM_DISTANCE_RATE);
			}

			bool ItemIsTooHigh() {
				return ItemIsTooFar(_groundItemPositionTmp.y, _playerTransform.position.y, _groundItemSize.y * MAX_PLAYER_TO_GROUND_ITEM_DISTANCE_RATE);
			}

			bool ItemIsTooFar(float higherValuePosition, float lowerValuePosition, float maxDistance) {
				return higherValuePosition - lowerValuePosition > maxDistance;
			}
		}

	}

}