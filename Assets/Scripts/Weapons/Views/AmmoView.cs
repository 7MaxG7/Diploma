using System;
using Photon.Pun;
using UnityEngine;


namespace Infrastructure {

	internal class AmmoView : MonoBehaviour {
		public event Action<Collider2D> OnTriggerEntered;
		public event Action OnBecomeInvisible;
		
		[SerializeField] private GameObject _gameObject;
		[SerializeField] private Transform _transform;
		[SerializeField] private PhotonView _photonView;
		[SerializeField] private Rigidbody2D _rigidBody;

		public GameObject GameObject => _gameObject;
		public Transform Transform => _transform;
		public PhotonView PhotonView => _photonView;
		public Rigidbody2D RigidBody => _rigidBody;


		private void OnTriggerEnter2D(Collider2D other) {
			OnTriggerEntered?.Invoke(other);
		}

		private void OnTriggerStay2D(Collider2D other) {
			OnTriggerEntered?.Invoke(other);
		}

		private void OnBecameInvisible() {
			OnBecomeInvisible?.Invoke();
		}
	}

}