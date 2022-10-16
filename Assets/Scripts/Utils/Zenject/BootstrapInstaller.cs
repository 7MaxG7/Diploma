using Services;
using Sounds;
using UI;
using UnityEngine;
using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class BootstrapInstaller : MonoInstaller {
		[SerializeField] private PermanentUiView _permanentUiPref;

		public override void InstallBindings() {
			var permanentUiView = Container.InstantiatePrefabForComponent<IPermanentUiView>(_permanentUiPref);

			Container.Bind<IPermanentUiView>().FromInstance(permanentUiView).AsSingle();
			Container.Bind<IGame>().To<Game>().AsSingle();
			Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
			Container.Bind<IPermanentUiController>().To<PermanentUiController>().AsSingle();
			Container.Bind<IRandomManager>().To<RandomManager>().AsSingle();
			Container.Bind<ISoundController>().To<SoundController>().AsSingle();
			Container.Bind<IPhotonManager>().To<PhotonManager>().AsSingle();
			Container.Bind<IPlayfabManager>().To<PlayfabManager>().AsSingle();
			Container.Bind<IPlayerPrefsService>().To<PlayerPrefsService>().AsSingle();
			Container.Bind<IScreenService>().To<ScreenService>().AsSingle();
		}
	}

}