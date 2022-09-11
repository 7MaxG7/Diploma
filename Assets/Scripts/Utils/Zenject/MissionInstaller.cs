using Abstractions;
using Controllers;
using Services;
using UI;
using UnityEngine;
using Zenject;


namespace Infrastructure.Zenject {

	internal sealed class MissionInstaller : MonoInstaller {
		public override void InstallBindings() {
			if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
				Container.Bind<IInputService>().To<PcInputService>().AsSingle();
			else
				Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
			Container.Bind<IPlayerMoveManager>().To<PlayerMoveManager>().AsSingle();
			Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();
			Container.Bind<IMissionMapManager>().To<MissionMapManager>().AsSingle();
			Container.Bind<IMapWrapper>().To<MapWrapper>().AsSingle();
			Container.Bind<IUnitsPool>().To<UnitsPool>().AsSingle();
			Container.Bind<IMonstersSpawner>().To<MonstersSpawner>().AsSingle();
			Container.Bind<IMonstersMoveManager>().To<MonstersMoveManager>().AsSingle();
			Container.Bind<IPhotonDataExchangeController>().To<PhotonDataExchangeController>().AsSingle();
			Container.Bind<IPhotonObjectsSynchronizer>().To<PhotonObjectsSynchronizer>().AsSingle();
			Container.Bind<IMissionUiController>().To<MissionUiController>().AsSingle();
			Container.Bind<IAmmosPool>().To<AmmosPool>().AsSingle();
			Container.Bind<IWeaponsManager>().To<WeaponsManager>().AsSingle();
			Container.Bind<IHandleDamageManager>().To<HandleDamageManager>().AsSingle();
			Container.Bind<ISkillsManager>().To<SkillsManager>().AsSingle();
			Container.Bind<IMissionResultManager>().To<MissionResultManager>().AsSingle();
			Container.Bind<IPlayersInteractionManager>().To<PlayersInteractionManager>().AsSingle();
			Container.Bind<ICompassManager>().To<CompassManager>().AsSingle();

			Container.Bind<IControllersHolder>().To<ControllersHolder>().AsSingle();
		}
	}

}