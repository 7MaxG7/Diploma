using Photon.Pun;
using Units;
using UnityEngine;


namespace Infrastructure {

	internal class Ammo : IAmmo {
		public GameObject GameObject => _ammoView.GameObject;
		public Transform Transform => _ammoView.Transform;
		public PhotonView PhotonView => _ammoView.PhotonView;
		public Rigidbody2D RigidBody => _ammoView.RigidBody;
		public int PoolIndex { get; }

		private int[] _damage;
		private float _damageTicksCooldown;
		private bool _isPiercing;
		
		private readonly AmmoView _ammoView;
		private IUnit _owner;
		private IAmmosPool _ammosPool;
		private IHandleDamageController _handleDamageController;


		public Ammo(GameObject ammoGo, IAmmosPool ammosPool, IHandleDamageController handleDamageController, int poolIndex) {
			PoolIndex = poolIndex;
			_ammosPool = ammosPool;
			_handleDamageController = handleDamageController;
			_ammoView = ammoGo.GetComponent<AmmoView>();
			_ammoView.OnTriggerEntered += HandleCollision;
			_ammoView.OnBecomeInvisible += DeactivateObj;
		}

		public void Dispose() {
			_ammoView.OnTriggerEntered -= HandleCollision;
			_ammoView.OnBecomeInvisible -= DeactivateObj;
			PhotonNetwork.Destroy(_ammoView.GameObject);
			_ammosPool = null;
			_handleDamageController = null;
			_owner = null;
		}

		public void Init(IUnit owner, int[] damage, float damageTicksCooldown = 0, bool isPiercing = false) {
			_owner = owner;
			_damage = damage;
			if (_damage.Length > 1) {
				_damageTicksCooldown = damageTicksCooldown;
			}
			_isPiercing = isPiercing;
		}

		public void Respawn(Vector2 spawnPosition) {
			Transform.position = _owner.Transform.position;
		}

		public void StopObj() {
			RigidBody.velocity = Vector2.zero;
		}

		private void HandleCollision(Collider2D collider) {
			if (collider.TryGetComponent<IDamagableView>(out var damageTaker) && !_owner.CheckOwnView(damageTaker)) {
				if (_damage.Length == 1)
					_handleDamageController.DealPermanentDamage(damageTaker, _damage[0], _owner);
				else if (_damage.Length > 1)
					_handleDamageController.DealPeriodicalDamage(damageTaker, _damage, _damageTicksCooldown, _owner);
				
				if (!_isPiercing)
					_ammosPool.ReturnObject(this);
			}
		}

		private void DeactivateObj() {
			_ammosPool.ReturnObject(this);
		}

	}

}