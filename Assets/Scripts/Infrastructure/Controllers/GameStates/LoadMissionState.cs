using System;
using Controllers;
using Infrastructure.Zenject;
using Photon.Pun;
using Units;
using UnityEngine;
using Utils;
using Zenject;


namespace Infrastructure {

	internal class LoadMissionState : ILoadMissionState {
		
		public event Action OnStateChange;
		
		private readonly ISceneLoader _sceneLoader;
		private readonly IPermanentUiController _permanentUiController;
		private readonly IPlayerMoveController _playerMoveController;
		private readonly ICameraController _cameraController;
		private readonly IMissionMapController _missionMapController;
		private readonly IMapWrapper _mapWrapper;
		private readonly IUnitsFactory _unitsFactory;
		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IMonstersMoveController _monstersMoveController;
		private readonly MissionConfig _missionConfig;
		private readonly PlayerConfig _playerConfig;


		[Inject]
		public LoadMissionState(ISceneLoader sceneLoader, IPermanentUiController permanentUiController, IMapWrapper mapWrapper, IUnitsFactory unitsFactory
				, IPlayerMoveController playerMoveController, ICameraController cameraController, IMissionMapController missionMapController
				, IMonstersSpawner monstersSpawner, IMonstersMoveController monstersMoveController, MissionConfig missionConfig, PlayerConfig playerConfig) {
			_sceneLoader = sceneLoader;
			_permanentUiController = permanentUiController;
			_unitsFactory = unitsFactory;
			_playerMoveController = playerMoveController;
			_cameraController = cameraController;
			_missionMapController = missionMapController;
			_monstersSpawner = monstersSpawner;
			_monstersMoveController = monstersMoveController;
			_mapWrapper = mapWrapper;
			_missionConfig = missionConfig;
			_playerConfig = playerConfig;
		}

		public void Enter(string sceneName) {
			_sceneLoader.LoadMissionScene(sceneName, PrepareScene);

			
			void PrepareScene() {
				var groundItemSize = _missionConfig.GroundItemPref.GetComponent<SpriteRenderer>().size;
				InitMapWrapper(groundItemSize);
				var player = PreparePlayer(groundItemSize);
				_cameraController.Follow(player.Transform, new Vector3(0, 0, -1));
				_missionMapController.Init(player.Transform, groundItemSize);
				_monstersSpawner.Init();
				_monstersMoveController.Init(player.Transform);
				OnStateChange?.Invoke();
			}
			
			void InitMapWrapper(Vector2 groundSize) {
				var horizontalZonesAmount = (PhotonNetwork.CurrentRoom.PlayerCount + 1) / 2;
				var verticalZonesAmount = PhotonNetwork.CurrentRoom.PlayerCount > 1 ? 2 : 1;
				var mapSize = new Vector2(
						groundSize.x * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * horizontalZonesAmount
						, groundSize.y * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * verticalZonesAmount
				);
				_mapWrapper.Init(Vector2.zero, mapSize);
			}
			
			IUnit PreparePlayer(Vector2 groundItemSize) {
				// ReSharper disable once PossibleLossOfFraction
				var xPosition = ((PhotonNetwork.LocalPlayer.ActorNumber - 1) / 2 + .5f) * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * groundItemSize.x;
				var yPosition = ((PhotonNetwork.LocalPlayer.ActorNumber - 1) % 2 + .5f) * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * groundItemSize.y;

				var player = _unitsFactory.CreatePlayer(new Vector2(xPosition, yPosition));
				_playerMoveController.Init(player);
				return player;
			}
		}

		public void Exit() {
			_permanentUiController.HideLoadingCurtain(isForced: true);
		}
	}

}