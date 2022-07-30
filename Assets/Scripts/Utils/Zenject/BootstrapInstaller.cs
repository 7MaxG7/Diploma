using UI;
using UnityEngine;
using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class BootstrapInstaller : MonoInstaller {
		[SerializeField] private PermanentUiView _permanentUiPref;

		public override void InstallBindings() {
			var permanentUiView = Container.InstantiatePrefabForComponent<IPermanentUiView>(_permanentUiPref);

			Container.Bind<IGame>().To<Game>().AsSingle();
			Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
			Container.Bind<IPermanentUiView>().FromInstance(permanentUiView).AsSingle();
			Container.Bind<IPermanentUiController>().To<PermanentUiController>().AsSingle();
			Container.Bind<IRandomController>().To<RandomController>().AsSingle();
			Container.Bind<ISoundController>().To<SoundController>().AsSingle();
		}
	}

}