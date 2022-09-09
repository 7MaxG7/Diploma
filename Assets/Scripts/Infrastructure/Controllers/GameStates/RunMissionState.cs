using System;
using Services;
using Zenject;


namespace Infrastructure {

	internal class RunMissionState : IRunMissionState {
		public event Action OnStateChange;
		
		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IWeaponsController _weaponsController;
		private readonly ISoundManager _soundManager;
		private readonly IMissionResultController _missionResultController;


		[Inject]
		public RunMissionState(IMonstersSpawner monstersSpawner, IWeaponsController weaponsController, ISoundManager soundManager
				, IMissionResultController missionResultController) {
			_monstersSpawner = monstersSpawner;
			_weaponsController = weaponsController;
			_soundManager = soundManager;
			_missionResultController = missionResultController;
		}
		
		public void Enter() {
			_soundManager.PlayRandomMissionMusic();
			_monstersSpawner.StartSpawn();
			_weaponsController.AddWeapon(WeaponType.SmallOrb);
			_weaponsController.StartShooting();
			_missionResultController.OnGameLeft += SwitchState;
		}

		public void Exit() {
			_missionResultController.OnGameLeft -= SwitchState;
			_monstersSpawner.KillMonstersAndStopSpawn();
			_weaponsController.StopShooting();
			_soundManager.StopAll();
		}

		private void SwitchState() {
			OnStateChange?.Invoke();
		}
	}

}