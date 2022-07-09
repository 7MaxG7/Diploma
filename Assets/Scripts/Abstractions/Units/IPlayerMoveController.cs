using Infrastructure;
using Units;


namespace Controllers {

	internal interface IPlayerMoveController : IFixedUpdater {
		void Init(IUnit player);
	}

}