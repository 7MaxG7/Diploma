using System;
using Services;
using Zenject;


namespace Infrastructure {

	internal class RunMissionState : IRunMissionState {
		public event Action OnStateChange;
		
		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IWeaponsController _weaponsController;
		private readonly ISoundController _soundController;


		[Inject]
		public RunMissionState(IMonstersSpawner monstersSpawner, IWeaponsController weaponsController, ISoundController soundController) {
			_monstersSpawner = monstersSpawner;
			_weaponsController = weaponsController;
			_soundController = soundController;
		}
		
		public void Enter() {
			_soundController.PlayRandomMissionMusic();
			_weaponsController.AddWeapon(WeaponType.SmallOrb);
			_monstersSpawner.StartSpawn();
			_weaponsController.StartShooting();
		}

		public void Exit() {
			_soundController.StopAll();
		}

	}

}