using System;
using Infrastructure;
using Units;


namespace Abstractions.Services {

	internal interface ICompassController : IUpdater, IDisposable {
		void Init(IUnit player);
	}

}