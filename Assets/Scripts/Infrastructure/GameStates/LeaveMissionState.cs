using System;
using Abstractions;
using Controllers;
using Services;
using UI;
using Units;
using Utils;
using Weapons;
using Zenject;


namespace Infrastructure {

	internal sealed class LeaveMissionState : ILeaveMissionState {
		private readonly IPhotonObjectsSynchronizer _photonObjectsSynchronizer;
		private readonly IPhotonDataExchangeController _photonDataExchangeController;
		private readonly ICameraManager _cameraManager;
		private readonly IMissionMapManager _missionMapManager;
		private readonly IPlayerMoveManager _playerMoveManager;
		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IMonstersMoveManager _monstersMoveManager;
		private readonly IWeaponsManager _weaponsManager;
		private readonly ISkillsManager _skillsManager;
		private readonly IMissionUiController _missionUiController;
		private readonly IMissionResultManager _missionResultManager;
		private readonly IUnitsPool _unitsPool;
		private readonly IAmmosPool _ammosPool;
		private readonly IPermanentUiController _permanentUiController;
		private readonly IMapWrapper _mapWrapper;
		private readonly IUnitsFactory _unitsFactory;
		private readonly IPlayersInteractionManager _playersInteractionManager;
		private readonly ICompassManager _compassManager;
		private readonly IPhotonManager _photonManager;
		public event Action OnStateChange;

		
		[Inject]
		public LeaveMissionState(IPhotonObjectsSynchronizer photonObjectsSynchronizer, IPhotonDataExchangeController photonDataExchangeController
				, ICameraManager cameraManager, IMissionMapManager missionMapManager, IPlayerMoveManager playerMoveManager
				, IMonstersSpawner monstersSpawner, IMonstersMoveManager monstersMoveManager, IWeaponsManager weaponsManager
				, ISkillsManager skillsManager, IMissionUiController missionUiController, IMissionResultManager missionResultManager
				, IUnitsPool unitsPool, IAmmosPool ammosPool, IPermanentUiController permanentUiController, IMapWrapper mapWrapper
				, IUnitsFactory unitsFactory, IPlayersInteractionManager playersInteractionManager, ICompassManager compassManager
				, IPhotonManager photonManager) {
			_photonObjectsSynchronizer = photonObjectsSynchronizer;
			_photonDataExchangeController = photonDataExchangeController;
			_cameraManager = cameraManager;
			_missionMapManager = missionMapManager;
			_playersInteractionManager = playersInteractionManager;
			_compassManager = compassManager;
			_photonManager = photonManager;
			_playerMoveManager = playerMoveManager;
			_monstersSpawner = monstersSpawner;
			_monstersMoveManager = monstersMoveManager;
			_weaponsManager = weaponsManager;
			_skillsManager = skillsManager;
			_missionUiController = missionUiController;
			_missionResultManager = missionResultManager;
			_unitsPool = unitsPool;
			_ammosPool = ammosPool;
			_permanentUiController = permanentUiController;
			_mapWrapper = mapWrapper;
			_unitsFactory = unitsFactory;
		}

		public void Enter() {
			_permanentUiController.OnResultPanelClosed += SwitchState;
			_missionUiController.Dispose();
			_missionResultManager.Dispose();
			_skillsManager.Dispose();
			_mapWrapper.Dispose();
			_weaponsManager.Dispose();
			_monstersMoveManager.Dispose();
			_monstersSpawner.Dispose();
			_missionMapManager.Dispose();
			_cameraManager.Dispose();
			_playersInteractionManager.Dispose();
			_compassManager.Dispose();
			_playerMoveManager.Dispose();
			_photonObjectsSynchronizer.Dispose();
			_photonDataExchangeController.Dispose();
			_ammosPool.Dispose();
			_unitsPool.Dispose();
			_unitsFactory.Dispose();
			_photonManager.Disconnect();
		}

		public void Exit() {
			_permanentUiController.OnResultPanelClosed -= SwitchState;
		}

		private void SwitchState() {
			OnStateChange?.Invoke();
		}

	}

}