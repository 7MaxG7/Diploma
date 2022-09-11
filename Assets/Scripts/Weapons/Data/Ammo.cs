using Photon.Pun;
using Services;
using Units;
using UnityEngine;


namespace Infrastructure {

	internal sealed class Ammo : IAmmo {
		public Transform Transform => _ammoView.Transform;
		public PhotonView PhotonView => _ammoView.PhotonView;
		public int PoolIndex { get; }
		
		private readonly AmmoView _ammoView;
		private IUnit _owner;
		private IAmmosPool _ammosPool;
		private IHandleDamageManager _handleDamageManager;
		private readonly IViewsFactory _viewsFactory;

		private int[] _damage;
		private float _damageTicksCooldown;
		private bool _isPiercing;


		public Ammo(GameObject ammoGo, IAmmosPool ammosPool, IHandleDamageManager handleDamageManager, IViewsFactory viewsFactory
				, int poolIndex) {
			PoolIndex = poolIndex;
			_ammosPool = ammosPool;
			_handleDamageManager = handleDamageManager;
			_viewsFactory = viewsFactory;
			_ammoView = ammoGo.GetComponent<AmmoView>();
			_ammoView.OnTriggerEntered += HandleCollision;
			_ammoView.OnBecomeInvisible += DeactivateObj;
		}

		public void Dispose() {
			_ammoView.OnTriggerEntered -= HandleCollision;
			_ammoView.OnBecomeInvisible -= DeactivateObj;
			_viewsFactory.DestroyPhotonObj(PhotonView);
			_ammosPool = null;
			_handleDamageManager = null;
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

		public void Push(Vector3 power) {
			_ammoView.Push(power);
		}

		public void Respawn(Vector2 _) {
			_ammoView.Locate(_owner.Transform.position);
		}

		public void ToggleActivation(bool isActive) {
			_ammoView.ToggleActivation(isActive);
		}

		public void StopObj() {
			_ammoView.StopMoving();
		}

		private void HandleCollision(Collider2D collider) {
			if (collider.TryGetComponent<IDamagableView>(out var damageTaker) && !_owner.CheckOwnView(damageTaker)) {
				if (_damage.Length == 1)
					_handleDamageManager.DealPermanentDamage(damageTaker, _damage[0], _owner);
				else if (_damage.Length > 1)
					_handleDamageManager.DealPeriodicalDamage(damageTaker, _damage, _damageTicksCooldown, _owner);
				
				if (!_isPiercing)
					_ammosPool.ReturnObject(this);
			}
		}

		private void DeactivateObj() {
			_ammosPool.ReturnObject(this);
		}
	}

}