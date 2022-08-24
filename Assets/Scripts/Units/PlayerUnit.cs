﻿using Infrastructure;
using Photon.Pun;
using UnityEngine;


namespace Units {

	internal class PlayerUnit : Unit, IExperienceReciever {
		
		public PlayerUnit(GameObject playerGO, PlayerConfig playerConfig) 
				: base(playerConfig.BaseMoveSpeed, playerConfig.LevelHpParameters[0].Health, -1) {
			Experience = new Experience(playerConfig.LevelHpParameters[0].Level, playerConfig.LevelExpParameters);
			Health.SetLevelUpHpParams(playerConfig.LevelHpParameters);
			Experience.OnLevelUp += Health.AddLevelUpHealth;
			var playerView = playerGO.GetComponent<PlayerView>();
			playerView.OnDamageTake += TakeDamage;
			UnitView = playerView;
		}

		public override void Dispose() {
			UnitView.OnDamageTake -= TakeDamage;
			Experience.OnLevelUp -= Health.AddLevelUpHealth;
			PhotonNetwork.Destroy(UnitView.GameObject);
			base.Dispose();
		}

		private void TakeDamage(int damage, IUnit damager) {
			Health.TakeDamage(new DamageInfo(damage, damager, this));
		}

		public void RecieveExperience(int exp) {
			Experience.AddExp(exp);
		}

		protected override void KillView(DamageInfo damageInfo) {
			base.KillView(damageInfo);
			GameObject.SetActive(false);
		}
	}

}