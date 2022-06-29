using UnityEngine;
using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class ConfigsInstaller : MonoInstaller {
		[SerializeField] private LobbyConfig _lobbyConfig;
		[SerializeField] private MissionConfig _missionConfig;
		[SerializeField] private PlayerConfig _playerConfig;
		[SerializeField] private MonstersConfig monstersConfig;
		
		public override void InstallBindings() {
			Container.Bind<LobbyConfig>().FromScriptableObject(_lobbyConfig).AsSingle();
			Container.Bind<MissionConfig>().FromScriptableObject(_missionConfig).AsSingle();
			Container.Bind<PlayerConfig>().FromScriptableObject(_playerConfig).AsSingle();
			Container.Bind<MonstersConfig>().FromScriptableObject(monstersConfig).AsSingle();
		}
	}

}