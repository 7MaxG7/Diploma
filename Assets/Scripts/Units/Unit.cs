using System;
using Photon.Pun;
using Units.Views;
using UnityEngine;
// ReSharper disable InconsistentNaming


namespace Units {

	internal abstract class Unit : IUnit, IDisposable {
		protected UnitView _unitView;
		public float MoveSpeed { get; }
		protected Health Health { get; }

		public GameObject GameObject => _unitView.GameObject;
		public Transform Transform => _unitView.Transform;
		public PhotonView PhotonView => _unitView.PhotonView;
		public CharacterController CharacterController => _unitView.CharacterController;
		public bool IsDead => Health.CurrentHp <= 0;


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

		protected void KillMonster() {
			Health.Kill();
		}

		protected abstract void DestroyView();

	}

}