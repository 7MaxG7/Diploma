using System;
using Infrastructure;
using Units;


namespace Services {

	internal interface ICompassManager : IUpdater, IDisposable {
		void Init(IUnit player);
	}

}