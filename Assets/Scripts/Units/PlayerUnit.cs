using Infrastructure;
using Photon.Pun;
using UnityEngine;


namespace Units {

	internal class PlayerUnit : Unit {
		
		public PlayerUnit(GameObject playerGO, PlayerConfig playerConfig) : base(playerConfig.BaseMoveSpeed, playerConfig.BaseHp) {
			Experience = new Experience(1, playerConfig.LevelParameters);
			var playerView = playerGO.GetComponent<PlayerView>();
			playerView.OnDamageTake += TakeDamage;
			_unitView = playerView;
		}

		public override void Dispose() {
			base.Dispose();
			_unitView.OnDamageTake -= TakeDamage;
		}

		private void TakeDamage(int damage) {
			Health.TakeDamage(damage);
		}

		protected override void DestroyView() {
			PhotonNetwork.Destroy(GameObject);
		}
	}

}