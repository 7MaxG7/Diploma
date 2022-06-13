using UnityEngine;
using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class ConfigsInstaller : MonoInstaller {
		[SerializeField] private LobbyConfig _lobbyConfig;
		
		public override void InstallBindings() {
			Container.Bind<LobbyConfig>().FromScriptableObject(_lobbyConfig).AsSingle();
		}
	}

}