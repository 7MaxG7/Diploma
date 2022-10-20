using System;
using Infrastructure;
using Units;


namespace Services
{
    internal interface IMonstersSpawner : IUpdater, IDisposable
    {
        bool SpawnIsOn { get; }

        void Init(IUnit player);
        void StartSpawn();
        void KillMonstersAndStopSpawn();
    }
}