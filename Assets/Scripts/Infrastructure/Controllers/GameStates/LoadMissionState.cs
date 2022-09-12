using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions;
using Controllers;
using Services;
using UI;
using Units;
using UnityEngine;
using Utils;
using Weapons;
using Zenject;
using Object = UnityEngine.Object;


namespace Infrastructure {

	internal sealed class LoadMissionState : ILoadMissionState {
		
		public event Action OnStateChange;
		
		private readonly ISceneLoader _sceneLoader;
		private readonly IPermanentUiController _permanentUiController;
		private readonly IPlayerMoveManager _playerMoveManager;
		private readonly ICameraManager _cameraManager;
		private readonly IMissionMapManager _missionMapManager;
		private readonly IMapWrapper _mapWrapper;
		private readonly IUnitsFactory _unitsFactory;
		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IMonstersMoveManager _monstersMoveManager;
		private readonly IMissionUiController _missionUiController;
		private readonly IPhotonDataExchangeController _photonDataExchangeController;
		private readonly IPhotonObjectsSynchronizer _photonObjectsSynchronizer;
		private readonly IWeaponsManager _weaponsManager;
		private readonly ISkillsManager _skillsManager;
		private readonly IMissionResultManager _missionResultManager;
		private readonly IPlayersInteractionManager _playersInteractionManager;
		private readonly ICompassManager _compassManager;
		private readonly IUnitsPool _unitsPool;
		private readonly IPhotonManager _photonManager;
		private readonly IViewsFactory _viewsFactory;
		private readonly MissionConfig _missionConfig;


		[Inject]
		public LoadMissionState(ISceneLoader sceneLoader, IPermanentUiController permanentUiController, IMapWrapper mapWrapper
				, IUnitsFactory unitsFactory, IPlayerMoveManager playerMoveManager, ICameraManager cameraManager
				, IMissionMapManager missionMapManager, IMonstersSpawner monstersSpawner, IMonstersMoveManager monstersMoveManager
				, IMissionUiController missionUiController, IPhotonDataExchangeController photonDataExchangeController
				, IPhotonObjectsSynchronizer photonObjectsSynchronizer, IWeaponsManager weaponsManager, ISkillsManager skillsManager
				, IMissionResultManager missionResultManager, IPlayersInteractionManager playersInteractionManager
				, ICompassManager compassManager, IUnitsPool unitsPool, IPhotonManager photonManager, IViewsFactory viewsFactory, MissionConfig missionConfig) {
			_sceneLoader = sceneLoader;
			_permanentUiController = permanentUiController;
			_mapWrapper = mapWrapper;
			_unitsFactory = unitsFactory;
			_playersInteractionManager = playersInteractionManager;
			_compassManager = compassManager;
			_unitsPool = unitsPool;
			_photonManager = photonManager;
			_viewsFactory = viewsFactory;
			_playerMoveManager = playerMoveManager;
			_cameraManager = cameraManager;
			_missionMapManager = missionMapManager;
			_monstersSpawner = monstersSpawner;
			_monstersMoveManager = monstersMoveManager;
			_missionUiController = missionUiController;
			_photonDataExchangeController = photonDataExchangeController;
			_photonObjectsSynchronizer = photonObjectsSynchronizer;
			_weaponsManager = weaponsManager;
			_skillsManager = skillsManager;
			_missionResultManager = missionResultManager;
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
				groundItemSize = _missionConfig.GroundItemPref.SpriteRenderer.size;
				var horizontalZonesAmount = (_photonManager.GetRoomPlayersAmount() + 1) / 2;
				var verticalZonesAmount = _photonManager.GetRoomPlayersAmount() > 1 ? 2 : 1;
				var mapSize = new Vector2(
						groundItemSize.x * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * horizontalZonesAmount
						, groundItemSize.y * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * verticalZonesAmount
				);
				_mapWrapper.Init(Vector2.zero, mapSize);
			}

			async Task<IUnit> InitUnits(Vector2 groundSize) {
				var player = await PreparePlayer(groundSize);
				_monstersSpawner.Init(player);
				_monstersMoveManager.Init(player.Transform);
				_weaponsManager.StopShooting();
				_mapWrapper.SetCheckingTransform(player.Transform);
				_mapWrapper.AddDependingTransforms(_unitsPool.ActiveMonsters);
				return player;
			}

			async Task<IUnit> PreparePlayer(Vector2 groundSize) {
				// ReSharper disable once PossibleLossOfFraction
				var xPosition = ((_photonManager.GetPlayerActorNumber() - 1) / 2 + .5f) * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * groundSize.x;
				var yPosition = ((_photonManager.GetPlayerActorNumber() - 1) % 2 + .5f) * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * groundSize.y;

				var player = _unitsFactory.CreatePlayer(new Vector2(xPosition, yPosition));
				var enemyPlayers = await FindEnemyPlayersAsync(player);
				_playersInteractionManager.Init(player, enemyPlayers);
				_playerMoveManager.Init(player);
				_compassManager.Init(player);
				_cameraManager.Follow(player.Transform, _missionConfig.CameraOffset);
				_missionMapManager.Init(player.Transform, groundSize, out var groundItems);
				_weaponsManager.Init(player);
				_skillsManager.Init(player);
				_missionResultManager.Init(player);
				_mapWrapper.AddDependingTransforms(groundItems);
				return player;
			}

			async Task<List<PlayerView>> FindEnemyPlayersAsync(IUnit player) {
				var enemyPlayers = new PlayerView[] { };
				while (enemyPlayers.Length != _photonManager.GetRoomPlayersAmount()) {
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
				var minePhotonDataExchanger = _viewsFactory.CreatePhotonObj(_missionConfig.PhotonDataSynchronizerPath, Vector3.zero, Quaternion.identity)
						.GetComponent<PhotonDataExchanger>();
				var othersPhotonDataExchangers = await FindSynchronizers(minePhotonDataExchanger);
				_photonDataExchangeController.Init(minePhotonDataExchanger, othersPhotonDataExchangers);
				_photonObjectsSynchronizer.Init(player.UnitView as PlayerView);
			}

			async Task<List<PhotonDataExchanger>> FindSynchronizers(PhotonDataExchanger minePhotonDataExchanger) {
				var photonDataExchangers = new PhotonDataExchanger[] { };
				while (photonDataExchangers.Length != _photonManager.GetRoomPlayersAmount()) {
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
			_permanentUiController.HideLoadingCurtain(interruptCurrentAnimation: true);
		}
	}

}