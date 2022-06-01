using Controllers;
using Services;
using UnityEngine;
using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class ControllersInstaller : MonoInstaller {
		public override void InstallBindings() {
			if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
				Container.Bind<IInputService>().To<PcInputService>().AsSingle();
			else
				Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
			Container.Bind<IMoveController>().To<MoveController>().AsSingle();
			Container.Bind<ICameraController>().To<CameraController>().AsSingle();
			
			Container.Bind<IControllersHolder>().To<ControllersHolder>().AsSingle();
		}
	}

}