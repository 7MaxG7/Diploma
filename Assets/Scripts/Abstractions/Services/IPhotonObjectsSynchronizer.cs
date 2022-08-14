using System;
using Units;


namespace Services {

	internal interface IPhotonObjectsSynchronizer : IDisposable {
		void Init(PlayerView playerView);
	}

}