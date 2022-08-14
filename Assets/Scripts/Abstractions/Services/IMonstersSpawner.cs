using System;
using Infrastructure;


namespace Services {

	internal interface IMonstersSpawner : IUpdater, IDisposable {
		bool SpawnIsOn { get; }
		
		void Init();
		void StartSpawn();
		void KillMonstersAndStopSpawn();
	}

}