﻿using Abstractions;
using Abstractions.Services;
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
			Container.Bind<IPlayerMoveController>().To<PlayerMoveController>().AsSingle();
			Container.Bind<ICameraController>().To<CameraController>().AsSingle();
			Container.Bind<IMissionMapController>().To<MissionMapController>().AsSingle();
			Container.Bind<IMapWrapper>().To<MapWrapper>().AsSingle();
			Container.Bind<IUnitsPool>().To<UnitsPool>().AsSingle();
			Container.Bind<IMonstersSpawner>().To<MonstersSpawner>().AsSingle();
			Container.Bind<IMonstersMoveController>().To<MonstersMoveController>().AsSingle();
			Container.Bind<IPhotonDataExchangeController>().To<PhotonDataExchangeController>().AsSingle();
			Container.Bind<IPhotonObjectsSynchronizer>().To<PhotonObjectsSynchronizer>().AsSingle();
			Container.Bind<IMissionUiController>().To<MissionUiController>().AsSingle();
			Container.Bind<IAmmosPool>().To<AmmosPool>().AsSingle();
			Container.Bind<IWeaponsController>().To<WeaponsController>().AsSingle();
			Container.Bind<IHandleDamageController>().To<HandleDamageController>().AsSingle();
			Container.Bind<ISkillsController>().To<SkillsController>().AsSingle();
			Container.Bind<IMissionResultController>().To<MissionResultController>().AsSingle();
			Container.Bind<IPlayersInteractionController>().To<PlayersInteractionController>().AsSingle();
			Container.Bind<ICompassController>().To<CompassController>().AsSingle();

			Container.Bind<IControllersHolder>().To<ControllersHolder>().AsSingle();
		}
	}

}