using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controllers;
using Services;
using UI;
using Units;
using UnityEngine;
using Utils;
using Weapons;
using Zenject;
using Object = UnityEngine.Object;


namespace Infrastructure
{
    internal sealed class LoadMissionState : ILoadMissionState
    {
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
        private readonly IPhotonObjectsSynchronizer _photonObjectsSynchronizer;
        private readonly IWeaponsManager _weaponsManager;
        private readonly ISkillsManager _skillsManager;
        private readonly IMissionResultManager _missionResultManager;
        private readonly IPlayersInteractionManager _playersInteractionManager;
        private readonly ICompassManager _compassManager;
        private readonly IAmmosFactory _ammosFactory;
        private readonly IUnitsPool _unitsPool;
        private readonly IPhotonManager _photonManager;
        private readonly MissionConfig _missionConfig;
        private readonly IAssetProvider _assetProvider;


        [Inject]
        public LoadMissionState(ISceneLoader sceneLoader, IPermanentUiController permanentUiController,
            IMapWrapper mapWrapper, IUnitsFactory unitsFactory, IPlayerMoveManager playerMoveManager
            , ICameraManager cameraManager, IMissionMapManager missionMapManager
            , IMonstersSpawner monstersSpawner, IMonstersMoveManager monstersMoveManager
            , IMissionUiController missionUiController, IPhotonObjectsSynchronizer photonObjectsSynchronizer
            , IWeaponsManager weaponsManager, ISkillsManager skillsManager, IMissionResultManager missionResultManager
            , IPlayersInteractionManager playersInteractionManager, ICompassManager compassManager, IAmmosFactory ammosFactory
            , IUnitsPool unitsPool, IPhotonManager photonManager, MissionConfig missionConfig, IAssetProvider assetProvider)
        {
            _sceneLoader = sceneLoader;
            _permanentUiController = permanentUiController;
            _mapWrapper = mapWrapper;
            _unitsFactory = unitsFactory;
            _playersInteractionManager = playersInteractionManager;
            _compassManager = compassManager;
            _ammosFactory = ammosFactory;
            _unitsPool = unitsPool;
            _photonManager = photonManager;
            _playerMoveManager = playerMoveManager;
            _cameraManager = cameraManager;
            _missionMapManager = missionMapManager;
            _monstersSpawner = monstersSpawner;
            _monstersMoveManager = monstersMoveManager;
            _missionUiController = missionUiController;
            _photonObjectsSynchronizer = photonObjectsSynchronizer;
            _weaponsManager = weaponsManager;
            _skillsManager = skillsManager;
            _missionResultManager = missionResultManager;
            _missionConfig = missionConfig;
            _assetProvider = assetProvider;
        }

        public void Enter(string sceneName)
        {
            _assetProvider.WarmUpForState(GetType());
            _sceneLoader.LoadMissionScene(sceneName, PrepareSceneAsync);


            async void PrepareSceneAsync()
            {
                InitMapWrapper(out var groundItemSize);
                var player = await InitUnits(groundItemSize);
                await InitUi(player);
                await InitPlayersInterators();
                OnStateChange?.Invoke();
            }

            void InitMapWrapper(out Vector2 groundItemSize)
            {
                groundItemSize = _missionConfig.GroundItemPref.SpriteRenderer.size;
                var horizontalZonesAmount = (_photonManager.GetRoomPlayersAmount() + 1) / 2;
                var verticalZonesAmount = _photonManager.GetRoomPlayersAmount() > 1 ? 2 : 1;
                var mapSize = new Vector2(
                    groundItemSize.x * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * horizontalZonesAmount
                    , groundItemSize.y * Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * verticalZonesAmount
                );
                _mapWrapper.Init(Vector2.zero, mapSize);
            }

            async Task<IUnit> InitUnits(Vector2 groundSize)
            {
                var player = await PreparePlayer(groundSize);
                _photonObjectsSynchronizer.Init(player.UnitView);
                _monstersSpawner.Init(player);
                _monstersMoveManager.Init(player.Transform);
                _weaponsManager.StopShooting();
                _mapWrapper.SetCheckingTransform(player.Transform);
                _mapWrapper.AddDependingTransforms(_unitsPool.ActiveMonsters);
                return player;
            }

            async Task<IUnit> PreparePlayer(Vector2 groundSize)
            {
                // ReSharper disable once PossibleLossOfFraction
                var xPosition = ((_photonManager.GetPlayerActorNumber() - 1) / 2 + .5f) *
                                Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * groundSize.x;
                var yPosition = ((_photonManager.GetPlayerActorNumber() - 1) % 2 + .5f) *
                                Constants.GROUND_ITEMS_AMOUNT_PER_PLAYER_ZONE_LENGTH * groundSize.y;

                _unitsFactory.Init();
                var player = await _unitsFactory.CreateMyPlayerAsync(new Vector2(xPosition, yPosition), Quaternion.identity);
                _playersInteractionManager.Init(player);
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

            async Task<List<PlayerView>> FindEnemyPlayersAsync()
            {
                var enemyPlayers = Array.Empty<PlayerView>();
                // This objects are instantiated on other clients and automaticly appear and syncronize on this client
                // with photon, so we just have to wait when they appear here
                var isFirstTry = true;
                while (enemyPlayers.Length != _photonManager.GetRoomPlayersAmount())
                {
                    if (isFirstTry)
                    {
                        isFirstTry = false;
                    }
                    else
                    {
                        await Task.Yield();
                    }

                    enemyPlayers = Object.FindObjectsOfType<PlayerView>();
                }

                return enemyPlayers.Where(view => !view.PhotonView.IsMine).ToList();
            }

            async Task InitUi(IUnit player)
            {
                await _missionUiController.Init(player);
            }

            async Task InitPlayersInterators()
            {
                _ammosFactory.Init();
                await _playersInteractionManager.PrepareOtherPlayers(_unitsFactory);
            }
        }

        public void Exit()
        {
            _permanentUiController.HideLoadingCurtain(interruptCurrentAnimation: true);
        }
    }
}