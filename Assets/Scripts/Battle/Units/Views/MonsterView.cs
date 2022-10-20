using System;
using UnityEngine;


namespace Units
{
    internal sealed class MonsterView : UnitView
    {
        public event Action<Collision2D> OnTriggerEnter;


        private void OnCollisionEnter2D(Collision2D other)
        {
            OnTriggerEnter?.Invoke(other);
        }
    }
}