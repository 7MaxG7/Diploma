using System;
using Photon.Pun;
using Units.Views;
using UnityEngine;
// ReSharper disable InconsistentNaming


namespace Units {

	internal abstract class Unit : IUnit, IDisposable {
		public event Action<IUnit> OnDied;
		
		public float MoveSpeed { get; }
		public Health Health { get; set; }
		public Experience Experience { get; protected set; }
		public UnitView UnitView { get; protected set; }
		
		public Rigidbody2D Rigidbody => UnitView.RigidBody;
		public GameObject GameObject => UnitView.GameObject;
		public Transform Transform => UnitView.Transform;
		public PhotonView PhotonView => UnitView.PhotonView;
		public int PoolIndex { get; }
		public bool IsDead => Health.CurrentHp <= 0;


		protected Unit(float moveSpeed, int hp, int poolIndex) {
			PoolIndex = poolIndex;
			MoveSpeed = moveSpeed;
			Health = new Health(hp);
			Health.OnDied += KillView;
		}

		public virtual void Dispose() {
			Health.OnDied -= KillView;
			Health = null;
			Experience = null;
		}

		public void Respawn(Vector2 spawnPosition) {
			Health.Restore();
			Transform.position = spawnPosition;
		}

		public bool CheckOwnView(IDamagableView damageTaker) {
			if (damageTaker is UnitView unitView) {
				return unitView == UnitView;
			}
			return false;
		}

		public void StopObj() {
			Rigidbody.velocity = Vector2.zero;
		}

		public void KillUnit() {
			Health.Kill();
		}

		protected virtual void KillView() {
			OnDied?.Invoke(this);
		}

	}

}