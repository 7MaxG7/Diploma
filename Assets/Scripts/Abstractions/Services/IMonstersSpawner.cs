using System;
using Infrastructure;


namespace Services {

	internal interface IMonstersSpawner : IUpdater, IDisposable {
		void Init();
		void StartSpawn();
		void StopSpawn();
	}

}