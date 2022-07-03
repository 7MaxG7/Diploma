using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controllers;
using Photon.Pun;
using Services;
using UI;
using Units;
using UnityEngine;
using Utils;
using Zenject;
using Object = UnityEngine.Object;


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
		private readonly IUnitsPool _unitsPool;
		private readonly IMissionUiController _missionUiController;
		private readonly IPhotonDataExchangeController _photonDataExchangeController;
		private readonly IPhotonObjectsSynchronizer _photonObjectsSynchronizer;
		private readonly MissionConfig _missionConfig;
		private readonly UiConfig _uiConfig;


		[Inject]
		public LoadMissionState(ISceneLoader sceneLoader, IPermanentUiController permanentUiController, IMapWrapper mapWrapper, IUnitsFactory unitsFactory
				, IPlayerMoveController playerMoveController, ICameraController cameraController, IMissionMapController missionMapController
				, IMonstersSpawner monstersSpawner, IMonstersMoveController monstersMoveController, IUnitsPool unitsPool, IMissionUiController missionUiController
				, IPhotonDataExchangeController photonDataExchangeController, IPhotonObjectsSynchronizer photonObjectsSynchronizer
				, MissionConfig missionConfig, UiConfig uiConfig) {
			_sceneLoader = sceneLoader;
			_permanentUiController = permanentUiController;
			_unitsFactory = unitsFactory;
			_playerMoveController = playerMoveController;
			_cameraController = cameraController;
			_missionMapController = missionMapController;
			_monstersSpawner = monstersSpawner;
			_monstersMoveController = monstersMoveController;
			_unitsPool = unitsPool;
			_missionUiController = missionUiController;
			_photonDataExchangeController = photonDataExchangeController;
			_photonObjectsSynchronizer = photonObjectsSynchronizer;
			_mapWrapper = mapWrapper;
			_missionConfig = missionConfig;
			_uiConfig = uiConfig;
		}

		public void Enter(string sceneName) {
			_sceneLoader.LoadMissionScene(sceneName, PrepareSceneAsync);


			async void PrepareSceneAsync() {
				await InitPhotonDataControllersAsync();
				InitMapWrapper(out var groundItemSize);
				InitUnits(groundItemSize, out var player);
				InitUi(player);
				OnStateChange?.Invoke();
			}

			async Task InitPhotonDataControllersAsync() {
				var minePhotonDataExchanger = PhotonNetwork.Instantiate(_missionConfig.PhotonDataSynchronizerPath, Vector3.zero, Quaternion.identity)
						.GetComponent<PhotonDataExchanger>();
				var othersPhotonDataExchangers = await FindSynchronizers(minePhotonDataExchanger);
				_photonDataExchangeController.Init(minePhotonDataExchanger, othersPhotonDataExchangers);
				_photonObjectsSynchronizer.Init(_photonDataExchangeController, _unitsPool);
			}

			async Task<List<PhotonDataExchanger>> FindSynchronizers(PhotonDataExchanger minePhotonDataExchanger) {
				var photonDataExchangers = new PhotonDataExchanger[] { };
				while (photonDataExchangers.Length != PhotonNetwork.CurrentRoom.PlayerCount) {
					photonDataExchangers = Object.FindObjectsOfType<PhotonDataExchanger>();
					await Task.Yield();
				}
				var photonDataExchangersList = photonDataExchangers.ToList();
				photonDataExchangersList.Remove(minePhotonDataExchanger);
				return photonDataExchangersList;
			}

			void InitMapWrapper(out Vector2 groundItemSize) {
				groundItemSize = _missionConfig.GroundItemPref.GetComponent<SpriteRenderer>().size;
				var horizontalZonesAmount = (PhotonNetwork.CurrentRoom.PlayerCount + 1) / 2;
				var verticalZonesAmount = PhotonNetwork.CurrentRoom.PlayerCount > 1 ? 2 : 1;
				var mapSize = new Vector2(
						groundItemSize.x * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * horizontalZonesAmount
						, groundItemSize.y * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * verticalZonesAmount
				);
				_mapWrapper.Init(Vector2.zero, mapSize);
			}

			void InitUnits(Vector2 vector2, out IUnit player) {
				player = PreparePlayer(vector2);
				_cameraController.Follow(player.Transform, new Vector3(0, 0, -1));
				_missionMapController.Init(player.Transform, vector2);
				_monstersSpawner.Init();
				_monstersMoveController.Init(player.Transform);
			}

			IUnit PreparePlayer(Vector2 groundItemSize) {
				// ReSharper disable once PossibleLossOfFraction
				var xPosition = ((PhotonNetwork.LocalPlayer.ActorNumber - 1) / 2 + .5f) * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * groundItemSize.x;
				var yPosition = ((PhotonNetwork.LocalPlayer.ActorNumber - 1) % 2 + .5f) * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * groundItemSize.y;

				var player = _unitsFactory.CreatePlayer(new Vector2(xPosition, yPosition));
				_playerMoveController.Init(player);
				return player;
			}

			void InitUi(IUnit player) {
				_missionUiController.Init(player);
			}
		}

		public void Exit() {
			_permanentUiController.HideLoadingCurtain(isForced: true);
		}
	}

}