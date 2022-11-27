using System.Threading.Tasks;
using UnityEngine;


namespace Weapons
{
    internal interface IAmmosFactory
    {
        void Init();
        Task<IAmmo> CreateMyAmmoAsync(WeaponType weaponType, Vector2 position, Quaternion rotation);
        Task<IAmmo> CreateAmmoAsync(WeaponType weaponType, Vector2 spawnPosition, Quaternion rotation, bool isMine = false);
    }
}