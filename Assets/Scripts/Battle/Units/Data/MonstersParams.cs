using System;
using UnityEngine.AddressableAssets;


namespace Units
{
    [Serializable]
    internal sealed class MonstersParams
    {
        public int MonsterLevel;
        public AssetReference UnitPrefab;
        public float MoveSpeed;
        public int Hp;
        public int Damage;
        public int ExperienceOnKill;
    }
}