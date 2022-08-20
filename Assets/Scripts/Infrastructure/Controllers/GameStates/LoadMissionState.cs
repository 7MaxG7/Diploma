using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Services;
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
		private readonly IMissionUiController _missionUiController;
		private readonly IPhotonDataExchangeController _photonDataExchangeController;
		private readonly IPhotonObjectsSynchronizer _photonObjectsSynchronizer;
		private readonly IWeaponsController _weaponsController;
		private readonly ISkillsController _skillsController;
		private readonly IMissionResultController _missionResultController;
		private readonly IPlayersInteractionController _playersInteractionController;
		private readonly ICompassController _compassController;
		private readonly MissionConfig _missionConfig;


		[Inject]
		public LoadMissionState(ISceneLoader sceneLoader, IPermanentUiController permanentUiController, IMapWrapper mapWrapper
				, IUnitsFactory unitsFactory, IPlayerMoveController playerMoveController, ICameraController cameraController
				, IMissionMapController missionMapController, IMonstersSpawner monstersSpawner, IMonstersMoveController monstersMoveController
				, IMissionUiController missionUiController, IPhotonDataExchangeController photonDataExchangeController
				, IPhotonObjectsSynchronizer photonObjectsSynchronizer, IWeaponsController weaponsController, ISkillsController skillsController
				, IMissionResultController missionResultController, IPlayersInteractionController playersInteractionController
				, ICompassController compassController, MissionConfig missionConfig) {
			_sceneLoader = sceneLoader;
			_permanentUiController = permanentUiController;
			_mapWrapper = mapWrapper;
			_unitsFactory = unitsFactory;
			_playersInteractionController = playersInteractionController;
			_compassController = compassController;
			_playerMoveController = playerMoveController;
			_cameraController = cameraController;
			_missionMapController = missionMapController;
			_monstersSpawner = monstersSpawner;
			_monstersMoveController = monstersMoveController;
			_missionUiController = missionUiController;
			_photonDataExchangeController = photonDataExchangeController;
			_photonObjectsSynchronizer = photonObjectsSynchronizer;
			_weaponsController = weaponsController;
			_skillsController = skillsController;
			_missionResultController = missionResultController;
			_missionConfig = missionConfig;
		}

		public void Enter(string sceneName) {
			_sceneLoader.LoadMissionScene(sceneName, PrepareSceneAsync);


			async void PrepareSceneAsync() {
				InitMapWrapper(out var groundItemSize);
				var player = await InitUnits(groundItemSize);
				await InitPhotonDataControllersAsync(player);
				InitUi(player);
				OnStateChange?.Invoke();
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

			async Task<IUnit> InitUnits(Vector2 groundSize) {
				var player = await PreparePlayer(groundSize);
				_monstersSpawner.Init(player);
				_monstersMoveController.Init(player.Transform);
				_weaponsController.StopShooting();
				return player;
			}

			async Task<IUnit> PreparePlayer(Vector2 groundSize) {
				// ReSharper disable once PossibleLossOfFraction
				var xPosition = ((PhotonNetwork.LocalPlayer.ActorNumber - 1) / 2 + .5f) * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * groundSize.x;
				var yPosition = ((PhotonNetwork.LocalPlayer.ActorNumber - 1) % 2 + .5f) * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * groundSize.y;

				var player = _unitsFactory.CreatePlayer(new Vector2(xPosition, yPosition));
				var enemyPlayers = await FindEnemyPlayersAsync(player);
				_playersInteractionController.Init(player, enemyPlayers);
				_playerMoveController.Init(player);
				_compassController.Init(player);
				_cameraController.Follow(player.Transform, _missionConfig.CameraOffset);
				_missionMapController.Init(player.Transform, groundSize);
				_weaponsController.Init(player);
				_skillsController.Init(player);
				_missionResultController.Init(player);
				return player;
			}

			async Task<List<PlayerView>> FindEnemyPlayersAsync(IUnit player) {
				var enemyPlayers = new PlayerView[] { };
				while (enemyPlayers.Length != PhotonNetwork.CurrentRoom.PlayerCount) {
					enemyPlayers = Object.FindObjectsOfType<PlayerView>();
					await Task.Yield();
				}
				var enemyPlayersList = enemyPlayers.ToList();
				var playerUnit = player.UnitView as PlayerView;
				if (playerUnit != null)
					enemyPlayersList.Remove(playerUnit);
				return enemyPlayersList;
			}

			async Task InitPhotonDataControllersAsync(IUnit player) {
				var minePhotonDataExchanger = PhotonNetwork.Instantiate(_missionConfig.PhotonDataSynchronizerPath, Vector3.zero, Quaternion.identity)
						.GetComponent<PhotonDataExchanger>();
				var othersPhotonDataExchangers = await FindSynchronizers(minePhotonDataExchanger);
				_photonDataExchangeController.Init(minePhotonDataExchanger, othersPhotonDataExchangers);
				_photonObjectsSynchronizer.Init(player.UnitView as PlayerView);
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

			void InitUi(IUnit player) {
				_missionUiController.Init(player);
			}
		}

		public void Exit() {
			_permanentUiController.HideLoadingCurtain(isForced: true);
		}
	}

}