using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Photon.Pun;


namespace Services {

	internal class PhotonObjectsSynchronizer : IPhotonObjectsSynchronizer, IDisposable {
		private readonly List<PhotonView> _otherPlayersObjects = new();
		private IPhotonDataExchangeController _photonDataExchangeController;
		private IUnitsPool _unitsPool;


		public void Dispose() {
			_unitsPool.OnObjectInstantiated -= SendInstantiationData;
			_unitsPool.OnObjectActivationToggle -= SendActivationToggleData;
			_photonDataExchangeController.OnInstantiationDataRecieved -= Register;
			_photonDataExchangeController.OnActivationDataRecieved -= SetActive;
		}

		public void Init(IPhotonDataExchangeController photonDataExchangeController, IUnitsPool unitsPool) {
			_photonDataExchangeController = photonDataExchangeController;
			_unitsPool = unitsPool;
			
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