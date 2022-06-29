using System;
using Zenject;


namespace Infrastructure {

	internal class RunMissionState : IRunMissionState {
		private readonly IMonstersSpawner _monstersSpawner;
		public event Action OnStateChange;


		[Inject]
		public RunMissionState(IMonstersSpawner monstersSpawner) {
			_monstersSpawner = monstersSpawner;
		}
		
		public void Enter() {
			_monstersSpawner.StartSpawn();
		}

		public void Exit() {
			
		}

	}

}