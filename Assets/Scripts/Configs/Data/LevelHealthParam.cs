using System;
using UnityEngine;


namespace Units
{
    [Serializable]
    internal class LevelHealthParam
    {
        [SerializeField] private int _level;
        [SerializeField] private int _health;

        public int Level => _level;
        public int Health => _health;
    }
}