using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class MainMenuInstaller : MonoInstaller {
		// [SerializeField] private MainMenuView _mainMenuPref;

		public override void InstallBindings() {
			// var mainMenuView = Container.InstantiatePrefabForComponent<MainMenuView>(_mainMenuPref);
			//
			// Container.Bind<MainMenuView>().FromInstance(mainMenuView).AsSingle().NonLazy();
			Container.Bind<IMainMenuController>().To<MainMenuController>().AsSingle().NonLazy();
		}
	}

}