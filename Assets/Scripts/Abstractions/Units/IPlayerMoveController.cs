using Infrastructure;
using Units;


namespace Controllers {

	internal interface IPlayerMoveController : IUpdater {
		void Init(IUnit player);
	}

}