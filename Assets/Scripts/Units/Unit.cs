using System;
using Photon.Pun;
using Units.Views;
using UnityEngine;
// ReSharper disable InconsistentNaming


namespace Units {

	internal abstract class Unit : IUnit, IDisposable {
		public float MoveSpeed { get; }
		public Health Health { get; }
		public Experience Experience { get; protected set; }
		public Rigidbody2D Rigidbody => _unitView.RigidBody;

		public GameObject GameObject => _unitView.GameObject;
		public Transform Transform => _unitView.Transform;
		public PhotonView PhotonView => _unitView.PhotonView;
		// public CharacterController CharacterController => _unitView.CharacterController;
		public bool IsDead => Health.CurrentHp <= 0;

		protected UnitView _unitView;
		

		protected Unit(float moveSpeed, int hp) {
			MoveSpeed = moveSpeed;
			Health = new Health(hp);
			Health.OnDied += DestroyView;
		}

		public virtual void Dispose() {
			Health.OnDied -= DestroyView;
		}

		public void Respawn(Vector2 spawnPosition) {
			Health.Restore();
			Transform.position = spawnPosition;
		}

		public bool CheckOwnView(IDamagableView damageTaker) {
			if (damageTaker is UnitView unitView) {
				return unitView == _unitView;
			}
			return false;
		}

		public void StopObj() {
			// CharacterController.Move(Vector3.zero);
			Rigidbody.velocity = Vector2.zero;
		}

		protected void KillMonster() {
			Health.Kill();
		}

		protected abstract void DestroyView();

	}

}