using System;
using Infrastructure;
using Units;


namespace Controllers {

	internal interface IPlayerMoveManager : IFixedUpdater, IDisposable {
		void Init(IUnit player);
	}

}