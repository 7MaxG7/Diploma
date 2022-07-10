using System;
using Units.Views;


namespace Units {

	internal class PlayerView : UnitView {
		public event Action<int> OnRecieveExperience;
	}

}