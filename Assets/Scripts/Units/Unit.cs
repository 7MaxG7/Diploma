using Photon.Pun;
using Units.Views;
using UnityEngine;


namespace Units {

	internal abstract class Unit : IUnit {
		protected UnitView _unitView;
		public Transform Transform { get; }
		public CharacterController CharacterController { get; }
		public float MoveSpeed { get; }
		protected Health Health { get; }
		public bool IsDead => Health.CurrentHp <= 0;


		protected Unit(GameObject playerGO, float moveSpeed, int hp) {
			MoveSpeed = moveSpeed;
			Transform = playerGO.transform;
			CharacterController = playerGO.GetComponent<CharacterController>();
			Health = new Health(hp);
			Health.OnDied += DestroyView;
		}

		protected void KillMonster() {
			Health.Kill();
		}

		private void DestroyView() {
			PhotonNetwork.Destroy(_unitView.gameObject);
		}
	}

}