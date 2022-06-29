using UnityEngine;


namespace Units {

	internal class PlayerUnit : Unit {
		public PlayerUnit(GameObject playerGO, float moveSpeed, int hp) : base(playerGO, moveSpeed, hp) {
			var playerView = playerGO.AddComponent<PlayerView>();
			playerView.OnDamageTake += TakeDamage;
		}

		private void TakeDamage(int damage) {
			
		}
	}

}