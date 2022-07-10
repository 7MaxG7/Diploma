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

		private readonly int[] _baseDamage;
		private readonly float _damageTicksCooldown;
		private readonly bool _isPiercing;
		
		private readonly AmmoView _ammoView;
		private IUnit _owner;
		private readonly IAmmosPool _ammosPool;
		private readonly IHandleDamageController _handleDamageController;


		public Ammo(GameObject ammoGo, WeaponsConfig.WeaponParam ammoParam, IAmmosPool ammosPool, IHandleDamageController handleDamageController) {
			_baseDamage = ammoParam.BaseDamage;
			if (_baseDamage.Length > 1) {
				_damageTicksCooldown = ammoParam.DamageTicksCooldown;
			}
			_isPiercing = ammoParam.IsPiercing;
			_ammosPool = ammosPool;
			_handleDamageController = handleDamageController;
			_ammoView = ammoGo.GetComponent<AmmoView>();
			_ammoView.OnTriggerEntered += HandleCollision;
			_ammoView.OnBecomeInvisible += DeactivateObj;
		}

		public void Dispose() {
			_ammoView.OnTriggerEntered -= HandleCollision;
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

		private void HandleCollision(Collider2D collider) {
			if (collider.TryGetComponent<IDamagableView>(out var damageTaker) && !_owner.CheckOwnView(damageTaker)) {
				if (_baseDamage.Length == 1)
					_handleDamageController.DealPermanentDamage(damageTaker, _baseDamage[0], _owner);
				else if (_baseDamage.Length > 1)
					_handleDamageController.DealPeriodicalDamage(damageTaker, _baseDamage, _damageTicksCooldown, _owner);
				
				if (!_isPiercing)
					_ammosPool.ReturnObject(this);
			}
		}

		private void DeactivateObj() {
			_ammosPool.ReturnObject(this);
		}

	}

}