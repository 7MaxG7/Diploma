using System;
using UnityEngine;
using Weapons;


namespace Sounds
{
    [Serializable]
    internal sealed class WeaponShootingClip
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private AudioClip _audioClip;

        public WeaponType WeaponType => _weaponType;
        public AudioClip AudioClip => _audioClip;
    }
}