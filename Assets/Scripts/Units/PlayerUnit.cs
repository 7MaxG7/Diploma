using Infrastructure;
using Photon.Pun;
using UnityEngine;


namespace Units {

	internal class PlayerUnit : Unit {
		
		public PlayerUnit(GameObject playerGO, PlayerConfig playerConfig) : base(playerConfig.BaseMoveSpeed, playerConfig.LevelHpParameters[0].Health) {
			Experience = new Experience(playerConfig.LevelHpParameters[0].Level, playerConfig.LevelExpParameters);
			Health.SetLevelUpHpParams(playerConfig.LevelHpParameters);
			Experience.OnLevelUp += Health.AddLevelUpHealth;
			var playerView = playerGO.GetComponent<PlayerView>();
			playerView.OnDamageTake += TakeDamage;
			playerView.OnRecieveExperience += RecieveExperience;
			_unitView = playerView;
		}

		public override void Dispose() {
			base.Dispose();
			_unitView.OnDamageTake -= TakeDamage;
			if (_unitView is PlayerView playerView)
				playerView.OnRecieveExperience -= RecieveExperience;
			Experience.OnLevelUp -= Health.AddLevelUpHealth;
		}

		private void TakeDamage(int damage) {
			Health.TakeDamage(damage);
		}

		private void RecieveExperience(int exp) {
			Experience.AddExp(exp);
		}

		protected override void DestroyView() {
			PhotonNetwork.Destroy(GameObject);
		}
	}

}