using Utils;
using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class FactoriesInstaller : MonoInstaller {
		public override void InstallBindings() {
			Container.Bind<IUnitsFactory>().To<UnitsFactory>().AsSingle();
			Container.Bind<IAmmosFactory>().To<AmmosFactory>().AsSingle();
		}
	}

}