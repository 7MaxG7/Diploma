using System;
using Services;
using Zenject;


namespace Infrastructure {

	internal sealed class RunMissionState : IRunMissionState {
		public event Action OnStateChange;
		
		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IWeaponsManager _weaponsManager;
		private readonly ISoundController _soundController;
		private readonly IMissionResultManager _missionResultManager;


		[Inject]
		public RunMissionState(IMonstersSpawner monstersSpawner, IWeaponsManager weaponsManager, ISoundController soundController
				, IMissionResultManager missionResultManager) {
			_monstersSpawner = monstersSpawner;
			_weaponsManager = weaponsManager;
			_soundController = soundController;
			_missionResultManager = missionResultManager;
		}
		
		public void Enter() {
			_soundController.PlayRandomMissionMusic();
			_monstersSpawner.StartSpawn();
			_weaponsManager.AddWeapon(WeaponType.SmallOrb);
			_weaponsManager.StartShooting();
			_missionResultManager.OnGameLeft += SwitchState;
		}

		public void Exit() {
			_missionResultManager.OnGameLeft -= SwitchState;
			_monstersSpawner.KillMonstersAndStopSpawn();
			_weaponsManager.StopShooting();
			_soundController.StopAll();
		}

		private void SwitchState() {
			OnStateChange?.Invoke();
		}
	}

}