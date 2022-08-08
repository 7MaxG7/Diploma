using System;
using Services;
using Zenject;


namespace Infrastructure {

	internal class RunMissionState : IRunMissionState {
		public event Action OnStateChange;
		
		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IWeaponsController _weaponsController;
		private readonly ISoundController _soundController;
		private readonly IMissionResultController _missionResultController;


		[Inject]
		public RunMissionState(IMonstersSpawner monstersSpawner, IWeaponsController weaponsController, ISoundController soundController
				, IMissionResultController missionResultController) {
			_monstersSpawner = monstersSpawner;
			_weaponsController = weaponsController;
			_soundController = soundController;
			_missionResultController = missionResultController;
		}
		
		public void Enter() {
			_soundController.PlayRandomMissionMusic();
			_monstersSpawner.StartSpawn();
			_weaponsController.AddWeapon(WeaponType.SmallOrb);
			_weaponsController.StartShooting();
			_missionResultController.OnGameLeft += SwitchState;
		}

		public void Exit() {
			_missionResultController.OnGameLeft -= SwitchState;
			_monstersSpawner.StopSpawn();
			_weaponsController.StopShooting();
			_soundController.StopAll();
		}

		private void SwitchState() {
			OnStateChange?.Invoke();
		}
	}

}