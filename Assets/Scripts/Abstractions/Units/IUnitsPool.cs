using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Units;
using UnityEngine;


namespace Services
{
    internal interface IUnitsPool : IDisposable
    {
        List<IUnit> ActiveMonsters { get; }

        Task<IUnit> SpawnObjectAsync(Vector2 position, Quaternion rotation, params object[] parameters);
    }
}