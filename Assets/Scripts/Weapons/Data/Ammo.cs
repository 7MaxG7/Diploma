using System;
using Photon.Pun;
using Units;
using UnityEngine;


namespace Infrastructure {

	internal class Ammo : IAmmo, IDisposable {
		public GameObject GameObject => _ammoView.GameObject;
		public Transform Transform => _ammoView.Transform;
		public PhotonView PhotonView => _ammoView.PhotonView;
		public Rigidbody2D RigidBody => _ammoView.RigidBody;

		private readonly int _baseDamage;
		
		private readonly AmmoView _ammoView;
		private IUnit _owner;
		private readonly IAmmosPool _ammosPool;


		public Ammo(GameObject ammoGo, int baseDamage, IAmmosPool ammosPool) {
			_baseDamage = baseDamage;
			_ammosPool = ammosPool;
			_ammoView = ammoGo.GetComponent<AmmoView>();
			_ammoView.OnTriggerEntered += DamageTriggeredUnit;
			_ammoView.OnBecomeInvisible += DeactivateObj;
		}

		public void Dispose() {
			_ammoView.OnTriggerEntered -= DamageTriggeredUnit;
			_ammoView.OnBecomeInvisible -= DeactivateObj;
		}

		public void Init(IUnit owner) {
			_owner = owner;
		}

		public void Respawn(Vector2 spawnPosition) {
			Transform.position = _owner.Transform.position;
		}

		public void StopObj() {
			RigidBody.velocity = Vector2.zero;
		}

		private void DamageTriggeredUnit(Collider2D collider) {
			if (collider.TryGetComponent<IDamagableView>(out var damageTaker)) {
				if (!_owner.CheckOwnView(damageTaker)) {
					_ammosPool.ReturnObject(this);
					damageTaker.TakeDamage(_baseDamage, _owner);
				}
			}
		}

		private void DeactivateObj() {
			_ammosPool.ReturnObject(this);
		}

	}

}