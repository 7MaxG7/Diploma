using System.Collections.Generic;
using System.Linq;
using Enums;
using Photon.Pun;
using Zenject;


namespace Services {

	internal class PhotonObjectsSynchronizer : IPhotonObjectsSynchronizer {
		private readonly List<PhotonView> _otherPlayersObjects = new();
		private readonly IPhotonDataExchangeController _photonDataExchangeController;
		private readonly IUnitsPool _unitsPool;


		[Inject]
		public PhotonObjectsSynchronizer(IPhotonDataExchangeController photonDataExchangeController, IUnitsPool unitsPool) {
			_photonDataExchangeController = photonDataExchangeController;
			_unitsPool = unitsPool;
		}
		
		public void Dispose() {
			_unitsPool.OnObjectInstantiated -= SendInstantiationData;
			_unitsPool.OnObjectActivationToggle -= SendActivationToggleData;
			_photonDataExchangeController.OnInstantiationDataRecieved -= Register;
			_photonDataExchangeController.OnActivationDataRecieved -= SetActive;
			_otherPlayersObjects.Clear();
		}

		public void Init() {
			_unitsPool.OnObjectInstantiated += SendInstantiationData;
			_unitsPool.OnObjectActivationToggle += SendActivationToggleData;
			_photonDataExchangeController.OnInstantiationDataRecieved += Register;
			_photonDataExchangeController.OnActivationDataRecieved += SetActive;
		}

		private void SendInstantiationData(int photonViewId) {
			_photonDataExchangeController.PrepareDataForSending(PhotonExchangerDataType.ObjectInstantiation, photonViewId);
		}

		private void SendActivationToggleData(int photonViewId, bool isActivated) {
			_photonDataExchangeController.PrepareDataForSending(PhotonExchangerDataType.ObjectActivation, photonViewId, isActivated);
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

	}

}