using Units.Views;
using UnityEngine;
// ReSharper disable InconsistentNaming


namespace Units {

	internal abstract class Unit : IUnit {
		protected UnitView _unitView;
		public GameObject GameObject => _unitView.gameObject;
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

		public void Respawn(Vector2 spawnPosition) {
			Health.Restore();
			Transform.position = spawnPosition;
			GameObject.SetActive(true);
		}

		protected void KillMonster() {
			Health.Kill();
		}

		protected abstract void DestroyView();
	}

}