using System;
using System.Threading.Tasks;
using UnityEngine;


namespace Weapons
{
    internal interface IAmmosPool : IDisposable
    {
        Task<IAmmo> SpawnObjectAsync(Vector2 spawnPosition, Quaternion rotation, params object[] parameters);
    }
}