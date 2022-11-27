using System;
using Photon.Pun;
using UnityEngine;

// ReSharper disable InconsistentNaming


namespace Units
{
    internal abstract class Unit : IUnit
    {
        public event Action<DamageInfo> OnDied;
        public event Action<PhotonView> OnDispose;

        public abstract IUnitView UnitView { get; }
        public abstract Transform Transform { get; }
        public abstract PhotonView PhotonView { get; }
        public Health Health { get; private set; }
        public Experience Experience { get; protected set; }
        public float MoveSpeed { get; }
        public int PoolIndex { get; }
        public bool IsDead => Health.CurrentHp <= 0;


        protected Unit(float moveSpeed, int hp, int poolIndex, bool isMine)
        {
            PoolIndex = poolIndex;
            MoveSpeed = moveSpeed;
            Health = new Health(hp);
            if (isMine)
                Health.OnDied += KillView;
        }

        public virtual void Dispose()
        {
            Health.OnDied -= KillView;
            OnDispose?.Invoke(PhotonView);
            Health = null;
            Experience = null;
        }

        public void Respawn(Vector2 spawnPosition)
        {
            Health.Restore();
            UnitView.Locate(spawnPosition);
        }

        public void ToggleActivation(bool isActive)
        {
            UnitView.ToggleActivation(isActive);
        }

        public bool CheckOwnView(IDamagableView damageTaker)
        {
            if (damageTaker is IUnitView unitView)
            {
                return unitView == UnitView;
            }

            return false;
        }

        public void Move(Vector3 deltaPosition)
        {
            UnitView.Move(deltaPosition);
        }

        public void StopObj()
        {
            UnitView.StopMoving();
        }

        public void KillUnit()
        {
            Health.Kill(this);
        }

        protected virtual void KillView(DamageInfo damageInfo)
        {
            OnDied?.Invoke(damageInfo);
        }
    }
}