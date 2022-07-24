using Infrastructure;


namespace Services {

	internal interface IMonstersSpawner : IUpdater {
		void Init();
		void StartSpawn();
		void StopSpawn();
	}

}