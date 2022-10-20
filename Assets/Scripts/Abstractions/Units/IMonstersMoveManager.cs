using System;
using Units;
using UnityEngine;


namespace Infrastructure
{
    internal interface IMonstersMoveManager : IFixedUpdater, IDisposable
    {
        void Init(Transform playerTransform);
        void RegisterMonster(IUnit enemy);
    }
}