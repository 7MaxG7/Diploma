using Infrastructure;
using Photon.Pun;
using UnityEngine;


namespace Units {

	internal class PlayerUnit : Unit, IExperienceReciever {
		
		public PlayerUnit(GameObject playerGO, PlayerConfig playerConfig) : base(playerConfig.BaseMoveSpeed, playerConfig.LevelHpParameters[0].Health, -1) {
			Experience = new Experience(playerConfig.LevelHpParameters[0].Level, playerConfig.LevelExpParameters);
			Health.SetLevelUpHpParams(playerConfig.LevelHpParameters);
			Experience.OnLevelUp += Health.AddLevelUpHealth;
			var playerView = playerGO.GetComponent<PlayerView>();
			playerView.OnDamageTake += TakeDamage;
			playerView.OnRecieveExperience += RecieveExperience;
			UnitView = playerView;
		}

		public override void Dispose() {
			base.Dispose();
			UnitView.OnDamageTake -= TakeDamage;
			if (UnitView is PlayerView playerView)
				playerView.OnRecieveExperience -= RecieveExperience;
			Experience.OnLevelUp -= Health.AddLevelUpHealth;
		}

		private void TakeDamage(int damage, IUnit damager) {
			Health.TakeDamage(damage);
		}

		public void RecieveExperience(int exp) {
			Experience.AddExp(exp);
		}

		protected override void KillView() {
			base.KillView();
			PhotonNetwork.Destroy(GameObject);
		}
	}

}