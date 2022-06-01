using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class BootstrapInstaller : MonoInstaller {
		
		public override void InstallBindings() {
			Container.Bind<IGame>().To<Game>().AsSingle();
			Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
			Container.Bind<IPermanentUiController>().To<PermanentUiController>().AsSingle();
		}
	}

}