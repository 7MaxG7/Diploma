using System.Collections.Generic;


namespace Services {

	internal interface IPhotonObjectsSynchronizer {
		void Init(List<PhotonDataExchanger> photonDataExchangers);
	}

}