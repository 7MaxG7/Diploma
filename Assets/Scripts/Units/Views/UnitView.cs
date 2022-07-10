using System;
using Photon.Pun;
using UnityEngine;


namespace Units.Views {

	internal abstract class UnitView : MonoBehaviour, IDamagableView {
		public event Action<int, IUnit> OnDamageTake;
		
		[SerializeField] private GameObject _gameObject;
		[SerializeField] private Transform _transform;
		[SerializeField] private PhotonView _photonView;
		[SerializeField] private Rigidbody2D _rigidBody;

		public GameObject GameObject => _gameObject;
		public Transform Transform => _transform;
		public PhotonView PhotonView => _photonView;
		public Rigidbody2D RigidBody => _rigidBody;

		public void TakeDamage(int damage, IUnit damager) {
			OnDamageTake?.Invoke(damage, damager);
		}
	}

}