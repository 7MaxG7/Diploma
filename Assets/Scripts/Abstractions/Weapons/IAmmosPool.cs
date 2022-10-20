using System;
using UnityEngine;


namespace Weapons
{
    internal interface IAmmosPool : IDisposable
    {
        event Action<int> OnObjectInstantiated;
        event Action<int, bool> OnObjectActivationToggle;

        IAmmo SpawnObject(Vector2 spawnPosition, params object[] parameters);
    }
}