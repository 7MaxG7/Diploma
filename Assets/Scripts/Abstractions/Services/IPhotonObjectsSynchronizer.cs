using System;


namespace Services {

	internal interface IPhotonObjectsSynchronizer : IDisposable {
		void Init(IPhotonDataExchangeController photonDataExchangers, IUnitsPool unitsPool);
	}

}