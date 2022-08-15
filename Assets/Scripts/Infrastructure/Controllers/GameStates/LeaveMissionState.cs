using System;
using Abstractions;
using Abstractions.Services;
using Controllers;
using Photon.Pun;
using Services;
using UI;
using Zenject;


namespace Infrastructure {

	internal class LeaveMissionState : ILeaveMissionState {
		private readonly IPhotonObjectsSynchronizer _photonObjectsSynchronizer;
		private readonly IPhotonDataExchangeController _photonDataExchangeController;
		private readonly ICameraController _cameraController;
		private readonly IMissionMapController _missionMapController;
		private readonly IPlayerMoveController _playerMoveController;
		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IMonstersMoveController _monstersMoveController;
		private readonly IWeaponsController _weaponsController;
		private readonly ISkillsController _skillsController;
		private readonly IMissionUiController _missionUiController;
		private readonly IMissionResultController _missionResultController;
		private readonly IUnitsPool _unitsPool;
		private readonly IAmmosPool _ammosPool;
		private readonly IPermanentUiController _permanentUiController;
		private readonly IPlayersInteractionController _playersInteractionController;
		private readonly ICompassController _compassController;
		public event Action OnStateChange;

		
		[Inject]
		public LeaveMissionState(IPhotonObjectsSynchronizer photonObjectsSynchronizer, IPhotonDataExchangeController photonDataExchangeController
				, ICameraController cameraController, IMissionMapController missionMapController, IPlayerMoveController playerMoveController
				, IMonstersSpawner monstersSpawner, IMonstersMoveController monstersMoveController, IWeaponsController weaponsController
				, ISkillsController skillsController, IMissionUiController missionUiController, IMissionResultController missionResultController
				, IUnitsPool unitsPool, IAmmosPool ammosPool, IPermanentUiController permanentUiController
				, IPlayersInteractionController playersInteractionController, ICompassController compassController) {
			_photonObjectsSynchronizer = photonObjectsSynchronizer;
			_photonDataExchangeController = photonDataExchangeController;
			_cameraController = cameraController;
			_missionMapController = missionMapController;
			_playersInteractionController = playersInteractionController;
			_compassController = compassController;
			_playerMoveController = playerMoveController;
			_monstersSpawner = monstersSpawner;
			_monstersMoveController = monstersMoveController;
			_weaponsController = weaponsController;
			_skillsController = skillsController;
			_missionUiController = missionUiController;
			_missionResultController = missionResultController;
			_unitsPool = unitsPool;
			_ammosPool = ammosPool;
			_permanentUiController = permanentUiController;
		}

		public void Enter() {
			_permanentUiController.OnResultPanelClosed += SwitchState;
			_missionUiController.Dispose();
			_missionResultController.Dispose();
			_skillsController.Dispose();
			_weaponsController.Dispose();
			_monstersMoveController.Dispose();
			_monstersSpawner.Dispose();
			_missionMapController.Dispose();
			_cameraController.Dispose();
			_playersInteractionController.Dispose();
			_compassController.Dispose();
			_playerMoveController.Player.Dispose();
			_playerMoveController.Dispose();
			_photonObjectsSynchronizer.Dispose();
			_photonDataExchangeController.Dispose();
			_ammosPool.Dispose();
			_unitsPool.Dispose();
			PhotonNetwork.Disconnect();
		}

		public void Exit() {
			_permanentUiController.OnResultPanelClosed -= SwitchState;
		}

		private void SwitchState() {
			OnStateChange?.Invoke();
		}

	}

}