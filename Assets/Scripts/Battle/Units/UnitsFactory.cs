using System.Threading.Tasks;
using Services;
using Units;
using UnityEngine;
using Zenject;


namespace Utils
{
    internal sealed class UnitsFactory : IUnitsFactory
    {
        private readonly IViewsFactory _viewsFactory;
        private readonly IPhotonManager _photonManager;
        private readonly IPunEventRaiser _punEventRaiser;
        private readonly PlayerConfig _playerConfig;
        private readonly MonstersConfig _monstersConfig;
        private IUnit _player;
        private Transform _root;


        [Inject]
        public UnitsFactory(IViewsFactory viewsFactory, IPhotonManager photonManager, IPunEventRaiser punEventRaiser
            , PlayerConfig playerConfig, MonstersConfig monstersConfig)
        {
            _viewsFactory = viewsFactory;
            _photonManager = photonManager;
            _punEventRaiser = punEventRaiser;
            _playerConfig = playerConfig;
            _monstersConfig = monstersConfig;
        }

        public void Dispose()
        {
            _player.OnDispose -= _photonManager.Destroy;
            _player.Dispose();
        }

        public async Task<IUnit> CreateMyPlayerAsync(Vector2 position, Quaternion rotation)
        {
            _player = await CreatePlayerAsync(position, rotation, true);
            _player.OnDispose -= _photonManager.Destroy;
            _punEventRaiser.RaisePlayerSpawn(_player.PhotonView, _player.Transform.position, _player.Transform.rotation);
            return _player;
        }

        public async Task<IUnit> CreateMyMonsterAsync(int level, Vector2 position, Quaternion rotation)
        {
            var monster = await CreateMonsterAsync(level, position, rotation, true);
            _punEventRaiser.RaiseMonsterCreation(monster.PhotonView, monster.Transform.position
                , monster.Transform.rotation, level);
            return monster;
        }

        public async Task<IUnit> CreatePlayerAsync(Vector2 position, Quaternion rotation, bool isMine = false)
        {
            _root ??= _viewsFactory.CreateGameObject(Constants.UNITS_ROOT_NAME).transform;
            var playerGo = await _viewsFactory.CreateGameObjectAsync(_playerConfig.PlayerPrefab, position, rotation, _root);
            return new PlayerUnit(playerGo, _playerConfig, isMine); 
        }

        public async Task<IUnit> CreateMonsterAsync(int monsterLevel, Vector2 spawnPosition, Quaternion rotation, bool isMine = false)
        {
            var monsterParams = _monstersConfig.GetMonsterParams(monsterLevel);
            _root ??= _viewsFactory.CreateGameObject(Constants.UNITS_ROOT_NAME).transform;
            var enemyGo = await _viewsFactory.CreateGameObjectAsync(monsterParams.UnitPrefab, spawnPosition, rotation, _root);
            return new MonsterUnit(enemyGo, monsterParams, monsterLevel, isMine);
        }
    }
}