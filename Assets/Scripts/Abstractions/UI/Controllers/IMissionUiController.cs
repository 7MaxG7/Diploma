using Infrastructure;
using Units;


namespace UI {

	internal interface IMissionUiController : IUpdater {
		void Init(IUnit player);
	}

}