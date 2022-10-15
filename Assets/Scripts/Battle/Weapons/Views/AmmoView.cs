using System;
using Photon.Pun;
using UnityEngine;


namespace Weapons {

	internal sealed class AmmoView : MonoBehaviour {
		[SerializeField] private GameObject _gameObject;
		[SerializeField] private Transform _transform;
		[SerializeField] private PhotonView _photonView;
		[SerializeField] private Rigidbody2D _rigidBody;
		
		public event Action<Collider2D> OnTriggerEntered;
		public event Action OnBecomeInvisible;
		
		public Transform Transform => _transform;
		public PhotonView PhotonView => _photonView;
		public GameObject GameObject => _gameObject;


		private void OnTriggerEnter2D(Collider2D other) {
			OnTriggerEntered?.Invoke(other);
		}

		private void OnBecameInvisible() {
			// If we change logic of returning to pool OnBecameInvisible, we should do return it to ppol in
			// Ammo.HandleCollision intead of just SetActive(false)
			OnBecomeInvisible?.Invoke();
		}

		public void Push(Vector2 power) {
			_rigidBody.AddForce(power, ForceMode2D.Impulse);
		}

		public void StopMoving() {
			_rigidBody.velocity = Vector2.zero;
		}

		public void Locate(Vector3 position) {
			_transform.position = position;
		}

		public void ToggleActivation(bool isActive) {
			_gameObject.SetActive(isActive);
		}
	}

}