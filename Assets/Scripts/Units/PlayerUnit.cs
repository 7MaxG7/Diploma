using Infrastructure;
using Photon.Pun;
using Services;
using Units.Views;
using UnityEngine;


namespace Units {

	internal sealed class PlayerUnit : Unit, IExperienceReciever {
		public override UnitView UnitView => _playerView;
		public override Transform Transform => _playerView.Transform;
		public override PhotonView PhotonView => _playerView.PhotonView;

		private readonly PlayerView _playerView;
		private readonly IViewsFactory _viewsFactory;

		public PlayerUnit(GameObject playerGO, IViewsFactory viewsFactory, PlayerConfig playerConfig) 
				: base(playerConfig.BaseMoveSpeed, playerConfig.LevelHpParameters[0].Health, -1) {
			_viewsFactory = viewsFactory;
			Experience = new Experience(playerConfig.LevelHpParameters[0].Level, playerConfig.LevelExpParameters);
			Health.SetLevelUpHpParams(playerConfig.LevelHpParameters);
			Experience.OnLevelUp += Health.AddLevelUpHealth;
			var playerView = playerGO.GetComponent<PlayerView>();
			playerView.OnDamageTake += TakeDamage;
			_playerView = playerView;
		}
		
		public override void Dispose() {
			_playerView.OnDamageTake -= TakeDamage;
			Experience.OnLevelUp -= Health.AddLevelUpHealth;
			_viewsFactory.DestroyPhotonObj(PhotonView);
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
			ToggleActivation(false);
		}
	}

}