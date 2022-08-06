using UnityEngine;
using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class ConfigsInstaller : MonoInstaller {
		[SerializeField] private MainMenuConfig mainMenuConfig;
		[SerializeField] private MissionConfig _missionConfig;
		[SerializeField] private PlayerConfig _playerConfig;
		[SerializeField] private MonstersConfig _monstersConfig;
		[SerializeField] private UiConfig _uiConfig;
		[SerializeField] private WeaponsConfig weaponsConfig;
		[SerializeField] private SoundConfig _soundConfig;
		
		public override void InstallBindings() {
			Container.Bind<MainMenuConfig>().FromScriptableObject(mainMenuConfig).AsSingle();
			Container.Bind<MissionConfig>().FromScriptableObject(_missionConfig).AsSingle();
			Container.Bind<PlayerConfig>().FromScriptableObject(_playerConfig).AsSingle();
			Container.Bind<MonstersConfig>().FromScriptableObject(_monstersConfig).AsSingle();
			Container.Bind<UiConfig>().FromScriptableObject(_uiConfig).AsSingle();
			Container.Bind<WeaponsConfig>().FromScriptableObject(weaponsConfig).AsSingle();
			Container.Bind<SoundConfig>().FromScriptableObject(_soundConfig).AsSingle();
		}
	}

}