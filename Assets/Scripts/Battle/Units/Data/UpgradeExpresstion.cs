using System;
using Infrastructure;
using UnityEngine;


namespace Weapons
{
    [Serializable]
    internal sealed class UpgradeExpresstion
    {
        [SerializeField] private WeaponStatType _statType;
        [SerializeField] private ArithmeticType _arithmetic;
        [SerializeField] private float _deltaValue;

        public WeaponStatType StatType => _statType;
        public ArithmeticType Arithmetic => _arithmetic;
        public float DeltaValue => _deltaValue;
    }
}