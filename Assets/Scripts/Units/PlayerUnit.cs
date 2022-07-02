using System;
using Photon.Pun;
using UnityEngine;


namespace Units {

	internal class PlayerUnit : Unit {
		public PlayerUnit(GameObject playerGO, float moveSpeed, int hp) : base(moveSpeed, hp) {
			var playerView = playerGO.GetComponent<PlayerView>();
			playerView.OnDamageTake += TakeDamage;
			_unitView = playerView;
		}

		public override void Dispose() {
			base.Dispose();
			_unitView.OnDamageTake -= TakeDamage;
		}

		private void TakeDamage(int damage) {
			
		}

		protected override void DestroyView() {
			PhotonNetwork.Destroy(GameObject);
		}
	}

}