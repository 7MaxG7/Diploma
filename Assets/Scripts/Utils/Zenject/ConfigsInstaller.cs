using Configs;
using Sounds;
using Units;
using UnityEngine;
using Weapons;
using Zenject;


namespace Infrastructure.Zenject
{
    internal sealed class ConfigsInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuConfig _mainMenuConfig;
        [SerializeField] private MissionConfig _missionConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private MonstersConfig _monstersConfig;
        [SerializeField] private UiConfig _uiConfig;
        [SerializeField] private WeaponsConfig _weaponsConfig;
        [SerializeField] private SoundConfig _soundConfig;
        [SerializeField] private AssetsConfig _assetsConfig;

        public override void InstallBindings()
        {
            Container.Bind<MainMenuConfig>().FromScriptableObject(_mainMenuConfig).AsSingle();
            Container.Bind<MissionConfig>().FromScriptableObject(_missionConfig).AsSingle();
            Container.Bind<PlayerConfig>().FromScriptableObject(_playerConfig).AsSingle();
            Container.Bind<MonstersConfig>().FromScriptableObject(_monstersConfig).AsSingle();
            Container.Bind<UiConfig>().FromScriptableObject(_uiConfig).AsSingle();
            Container.Bind<WeaponsConfig>().FromScriptableObject(_weaponsConfig).AsSingle();
            Container.Bind<SoundConfig>().FromScriptableObject(_soundConfig).AsSingle();
            Container.Bind<AssetsConfig>().FromScriptableObject(_assetsConfig).AsSingle();
        }
    }
}