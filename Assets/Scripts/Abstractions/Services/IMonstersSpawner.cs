namespace Infrastructure {

	internal interface IMonstersSpawner : IUpdater {
		void Init();
		void StartSpawn();
		void StopSpawn();

	}

}