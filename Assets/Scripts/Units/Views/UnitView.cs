using System;
using Photon.Pun;
using UnityEngine;


namespace Units.Views {

	internal abstract class UnitView : MonoBehaviour, IDamagableView {
		public event Action<int> OnDamageTake;
		
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private GameObject _gameObject;
		[SerializeField] private Transform _transform;
		[SerializeField] private PhotonView _photonView;

		public CharacterController CharacterController => _characterController;
		public GameObject GameObject => _gameObject;
		public Transform Transform => _transform;
		public PhotonView PhotonView => _photonView;

		public void TakeDamage(int damage) {
			OnDamageTake?.Invoke(damage);
		}
	}

}