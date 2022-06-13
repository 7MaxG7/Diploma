using UnityEngine;
using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class MainMenuInstaller : MonoInstaller {
		[SerializeField] private MainMenuView _mainMenuPref;

		public override void InstallBindings() {
			var mainMenuView = Container.InstantiatePrefabForComponent<IMainMenuView>(_mainMenuPref);
			
			Container.Bind<IMainMenuView>().FromInstance(mainMenuView).AsSingle();
			Container.Bind<IMainMenuController>().To<MainMenuController>().AsSingle();
		}
	}

}