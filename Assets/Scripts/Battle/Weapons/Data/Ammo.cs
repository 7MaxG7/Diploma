using System;
using Photon.Pun;
using Units;
using UnityEngine;


namespace Weapons
{
    internal sealed class Ammo : IAmmo
    {
        public event Action<IAmmo> OnLifetimeExpired;
        public event Action<IDamagableView, int[], float, IUnit> OnCollidedWithDamageTaker;
        public event Action<PhotonView> OnDispose;

        public Transform Transform => _ammoView.Transform;
        public PhotonView PhotonView => _ammoView.PhotonView;
        public int PoolIndex { get; }

        private readonly AmmoView _ammoView;
        private IUnit _owner;

        private int[] _damage;
        private float _damageTicksCooldown;
        private bool _isPiercing;


        public Ammo(GameObject ammoGo, int poolIndex)
        {
            PoolIndex = poolIndex;
            _ammoView = ammoGo.GetComponent<AmmoView>();
            _ammoView.OnTriggerEntered += HandleCollision;
            _ammoView.OnBecomeInvisible += DeactivateObj;
        }

        public void Dispose()
        {
            _ammoView.OnTriggerEntered -= HandleCollision;
            _ammoView.OnBecomeInvisible -= DeactivateObj;
            OnDispose?.Invoke(PhotonView);
            _owner = null;
        }

        public void Init(IUnit owner, int[] damage, float damageTicksCooldown = 0, bool isPiercing = false)
        {
            _owner = owner;
            _damage = damage;
            if (_damage.Length > 1)
            {
                _damageTicksCooldown = damageTicksCooldown;
            }

            _isPiercing = isPiercing;
        }

        public void Push(Vector3 power)
        {
            _ammoView.Push(power);
        }

        public void Respawn(Vector2 _)
        {
            _ammoView.Locate(_owner.Transform.position);
        }

        public void ToggleActivation(bool isActive)
        {
            _ammoView.ToggleActivation(isActive);
        }

        public void StopObj()
        {
            _ammoView.StopMoving();
        }

        private void HandleCollision(Collider2D collider)
        {
            if (collider.TryGetComponent<IDamagableView>(out var damageTaker) && !_owner.CheckOwnView(damageTaker))
            {
                OnCollidedWithDamageTaker?.Invoke(damageTaker, _damage, _damageTicksCooldown, _owner);

                if (!_isPiercing)
                    // On deactivating obj becomes invisible, so while ammo returnes to pool OnBecomeInvisible we've got
                    // to just deactivate it here to not return it to pool twice. If we change OnBecomeInvisible logic,
                    // we should do OnLifetimeExpired?.Invoke(this) here
                    _ammoView.GameObject.SetActive(false);
            }
        }

        private void DeactivateObj()
        {
            OnLifetimeExpired?.Invoke(this);
        }
    }
}