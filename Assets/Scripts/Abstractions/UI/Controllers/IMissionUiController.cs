using System;
using System.Threading.Tasks;
using Infrastructure;
using Units;
using UnityEngine;
using Weapons;


namespace UI
{
    internal interface IMissionUiController : IUpdater, IDisposable
    {
        event Action<WeaponType> OnSkillChoose;

        Task Init(IUnit player);
        void ShowSkillsChoose(ActualSkillInfo[] skills);
        void ShowCompass(Vector3 closestEnemyPlayerDestination);
        void HideCompass(Vector3 closestEnemyPlayerDestination);
    }
}