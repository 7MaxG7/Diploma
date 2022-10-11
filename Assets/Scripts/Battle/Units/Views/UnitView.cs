using System;
using Photon.Pun;
using UnityEngine;


namespace Units {

	internal abstract class UnitView : MonoBehaviour, IUnitView
	{
		[SerializeField] protected GameObject _gameObject;
		[SerializeField] protected Transform _transform;
		[SerializeField] private PhotonView _photonView;
		[SerializeField] private Rigidbody2D _rigidBody;

		public event Action<int, IUnit> OnDamageTake;
		
		public Transform Transform => _transform;
		public PhotonView PhotonView => _photonView;

		
		public void TakeDamage(int damage, IUnit damager) {
			OnDamageTake?.Invoke(damage, damager);
		}

		public virtual void Move(Vector3 deltaPosition) {
			_rigidBody.MovePosition(Transform.position + deltaPosition);
		}

		public void StopMoving() {
			_rigidBody.velocity = Vector2.zero;
		}

		public void Locate(Vector2 position) {
			_transform.position = position;
		}

		public void ToggleActivation(bool isActive) {
			_gameObject.SetActive(isActive);
		}
	}

}