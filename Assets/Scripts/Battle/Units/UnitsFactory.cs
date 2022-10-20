using Services;
using Units;
using Unity.Mathematics;
using UnityEngine;
using Zenject;


namespace Utils
{
    internal sealed class UnitsFactory : IUnitsFactory
    {
        private readonly IViewsFactory _viewsFactory;
        private readonly IPhotonManager _photonManager;
        private readonly PlayerConfig _playerConfig;
        private readonly MonstersConfig _monstersConfig;
        private PlayerUnit _player;


        [Inject]
        public UnitsFactory(IViewsFactory viewsFactory, IPhotonManager photonManager, PlayerConfig playerConfig
            , MonstersConfig monstersConfig)
        {
            _viewsFactory = viewsFactory;
            _photonManager = photonManager;
            _playerConfig = playerConfig;
            _monstersConfig = monstersConfig;
        }

        public void Dispose()
        {
            _player.OnDispose -= _photonManager.Destroy;
            _player.Dispose();
        }

        public IUnit CreatePlayer(Vector2 position)
        {
            var playerGo = _viewsFactory.CreatePhotonObj(_playerConfig.PlayerPrefabPath, position, quaternion.identity);
            _player = new PlayerUnit(playerGo, _playerConfig);
            _player.OnDispose += _photonManager.Destroy;
            return _player;
        }

        public IUnit CreateMonster(int monsterLevel, Vector2 spawnPosition)
        {
            var monsterParams = _monstersConfig.GetMonsterParams(monsterLevel);
            var enemyGo = _viewsFactory.CreatePhotonObj(monsterParams.PrefabPath, spawnPosition, Quaternion.identity);
            return new MonsterUnit(enemyGo, monsterParams, monsterLevel);
        }
    }
}