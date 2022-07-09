using System;
using Services;
using Zenject;


namespace Infrastructure {

	internal class RunMissionState : IRunMissionState {
		public event Action OnStateChange;
		
		private readonly IMonstersSpawner _monstersSpawner;
		private readonly IWeaponsController _weaponsController;


		[Inject]
		public RunMissionState(IMonstersSpawner monstersSpawner, IWeaponsController weaponsController) {
			_monstersSpawner = monstersSpawner;
			_weaponsController = weaponsController;
		}
		
		public void Enter() {
			_weaponsController.AddWeapon(WeaponType.Pistol);
			_monstersSpawner.StartSpawn();
			_weaponsController.StartShooting();
		}

		public void Exit() {
			
		}

	}

}