using System;
using System.Collections.Generic;
using Units;
using UnityEngine;


namespace Services
{
    internal interface IUnitsPool : IDisposable
    {
        event Action<int> OnObjectInstantiated;
        event Action<int, bool> OnObjectActivationToggle;
        List<IUnit> ActiveMonsters { get; }

        IUnit SpawnObject(Vector2 spawnPosition, params object[] parameters);
    }
}