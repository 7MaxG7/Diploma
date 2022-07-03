using System;
using System.Collections.Generic;
using Enums;


namespace Services {

	internal interface IPhotonDataExchangeController {
		void Init(PhotonDataExchanger minePhotonDataExchanger, List<PhotonDataExchanger> othersPhotonDataExchangers);

		void PrepareDataForSending(PhotonExchangerDataType dataType, params object[] data);

		event Action<int, bool> OnActivationDataRecieved;
		event Action<int> OnInstantiationDataRecieved;
	}

}