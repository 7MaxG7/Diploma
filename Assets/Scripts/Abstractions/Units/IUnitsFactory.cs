using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Units;
using UnityEngine;


namespace Utils
{
    internal interface IUnitsFactory : IDisposable
    {
        void Init();
        Task<IUnit> CreateMyPlayerAsync(Vector2 position, Quaternion rotation);
        Task<IUnit> CreateMyMonsterAsync(int level, Vector2 position, Quaternion rotation);
        Task<IUnit> CreatePlayerAsync(Vector2 position, Quaternion quaternion, bool isMine = false);
        Task<IUnit> CreateMonsterAsync(int monsterParams, Vector2 spawnPosition, Quaternion rotation, bool isMine = false);
        Task<HashSet<PlayerView>> GetOtherPlayers(int enemyPlayersAmount);
    }
}