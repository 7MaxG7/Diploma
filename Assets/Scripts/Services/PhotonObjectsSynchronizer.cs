using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;


namespace Services {

	internal class PhotonObjectsSynchronizer : IPhotonObjectsSynchronizer, IDisposable {
		private readonly List<PhotonView> _otherPlayersObjects = new();
		private List<PhotonDataExchanger> _photonDataExchangers;


		public void Dispose() {
			foreach (var dataExchanger in _photonDataExchangers) {
				dataExchanger.OnActivationDataRecieved -= SetActive;
				dataExchanger.OnInstantiationDataRecieved -= Register;
			}
		}

		public void Init(List<PhotonDataExchanger> photonDataExchangers) {
			_photonDataExchangers = photonDataExchangers;
			foreach (var dataExchanger in _photonDataExchangers) {
				dataExchanger.OnActivationDataRecieved += SetActive;
				dataExchanger.OnInstantiationDataRecieved += Register;
			}
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