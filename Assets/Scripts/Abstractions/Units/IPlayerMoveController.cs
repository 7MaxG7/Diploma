using System;
using Infrastructure;
using Units;


namespace Controllers {

	internal interface IPlayerMoveController : IFixedUpdater, IDisposable {
		IUnit Player { get; }
		
		void Init(IUnit player);
	}

}