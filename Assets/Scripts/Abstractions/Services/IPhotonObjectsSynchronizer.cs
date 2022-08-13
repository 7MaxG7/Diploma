using System;
using System.Collections.Generic;
using Units;


namespace Services {

	internal interface IPhotonObjectsSynchronizer : IDisposable {
		void Init(PlayerView playerView);
	}

}