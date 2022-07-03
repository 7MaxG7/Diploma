using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class MainMenuInstaller : MonoInstaller {

		public override void InstallBindings() {
			Container.Bind<IMainMenuController>().To<MainMenuController>().AsSingle().NonLazy();
		}
	}

}