using System.Collections.Generic;
using System.Linq;
using Enums;
using Infrastructure;
using Photon.Pun;
using Units;
using Zenject;


namespace Services {

	internal sealed class PhotonObjectsSynchronizer : IPhotonObjectsSynchronizer {
		private readonly IPhotonDataExchangeController _photonDataExchangeController;
		private readonly IUnitsPool _unitsPool;
		private readonly IAmmosPool _ammosPool;
		private readonly IHandleDamageManager _handleDamageManager;
		private PlayerView _playerView;
		private readonly List<PhotonView> _otherPlayersObjects = new();


		[Inject]
		public PhotonObjectsSynchronizer(IPhotonDataExchangeController photonDataExchangeController, IUnitsPool unitsPool, IAmmosPool ammosPool
				, IHandleDamageManager handleDamageManager) {
			_photonDataExchangeController = photonDataExchangeController;
			_unitsPool = unitsPool;
			_ammosPool = ammosPool;
			_handleDamageManager = handleDamageManager;
		}
		
		public void Dispose() {
			_unitsPool.OnObjectInstantiated -= SendInstantiationData;
			_unitsPool.OnObjectActivationToggle -= SendActivationToggleData;
			_ammosPool.OnObjectInstantiated -= SendInstantiationData;
			_ammosPool.OnObjectActivationToggle -= SendActivationToggleData;
			_handleDamageManager.OnDamageEnemyPlayer -= SendDamagingEnemyHeroData;
			_photonDataExchangeController.OnInstantiationDataRecieved -= Register;
			_photonDataExchangeController.OnActivationDataRecieved -= SetActive;
			_photonDataExchangeController.OnDamagePlayerDataRecieved -= DamagePlayer;
			_otherPlayersObjects.Clear();
		}

		public void Init(PlayerView playerView) {
			_playerView = playerView;
			_unitsPool.OnObjectInstantiated += SendInstantiationData;
			_unitsPool.OnObjectActivationToggle += SendActivationToggleData;
			_ammosPool.OnObjectInstantiated += SendInstantiationData;
			_ammosPool.OnObjectActivationToggle += SendActivationToggleData;
			_handleDamageManager.OnDamageEnemyPlayer += SendDamagingEnemyHeroData;
			_photonDataExchangeController.OnInstantiationDataRecieved += Register;
			_photonDataExchangeController.OnActivationDataRecieved += SetActive;
			_photonDataExchangeController.OnDamagePlayerDataRecieved += DamagePlayer;
		}

		private void SendInstantiationData(int photonViewId) {
			_photonDataExchangeController.PrepareDataForSending(PhotonExchangerDataType.ObjectInstantiation, photonViewId);
		}

		private void SendActivationToggleData(int photonViewId, bool isActivated) {
			_photonDataExchangeController.PrepareDataForSending(PhotonExchangerDataType.ObjectActivation, photonViewId, isActivated);
		}

		private void SendDamagingEnemyHeroData(PhotonDamageInfo photonDamageInfo) {
			_photonDataExchangeController.PrepareDataForSending(PhotonExchangerDataType.DamagingEnemyHero, photonDamageInfo.PhotonViewID
					, photonDamageInfo.Damage);
		}

		private void Register(int photonViewId) {
			foreach (var photonView in PhotonNetwork.PhotonViewCollection) {
				if (photonView.ViewID == photonViewId) {
					_otherPlayersObjects.Add(photonView);
					return;
				}
			}
		}

		private void SetActive(int photonViewId, bool isActive) {
			var photonView = _otherPlayersObjects.FirstOrDefault(obj => obj.ViewID == photonViewId);
			if (photonView != null)
				photonView.gameObject.SetActive(isActive);
		}

		private void DamagePlayer(int playerPhotonId, int damage) {
			if (_playerView.PhotonView.ViewID == playerPhotonId) {
				_playerView.TakeDamage(damage, null);
			}
		}
	}

}