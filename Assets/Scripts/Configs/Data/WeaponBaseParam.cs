using System;
using UnityEngine;


namespace Weapons
{
    [Serializable]
    internal class WeaponBaseParam : IWeaponDescription
    {
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private string _ammoPrefabPath;
        [SerializeField] private float _range;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _ammoSpeed;

        [Tooltip("Damage of each tick in case of periodical damage")] [SerializeField]
        private int[] _baseTicksDamage;

        [SerializeField] private float _damageTicksCooldown;
        [SerializeField] private bool _isPiercing;

        [Tooltip("Ui params")] [SerializeField]
        private string _name;

        [SerializeField] private string _description;

        public WeaponType WeaponType => _weaponType;
        public string AmmoPrefabPath => _ammoPrefabPath;
        public float Range => _range;
        public float Cooldown => _cooldown;
        public float AmmoSpeed => _ammoSpeed;
        public int[] BaseTicksDamage => _baseTicksDamage;
        public float DamageTicksCooldown => _damageTicksCooldown;
        public bool IsPiercing => _isPiercing;

        public string GetNameForLevel(int level)
        {
            return _name;
        }

        public string GetDescriptionForLevel(int level)
        {
            return _description;
        }
    }
}