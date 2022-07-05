using System;
using Units.Views;


namespace Units {

	internal class PlayerView : UnitView, IExperienceReciever {
		public event Action<int> OnRecieveExperience;

		public void RecieveExperience(int killExperience) {
			OnRecieveExperience?.Invoke(killExperience);
		}
	}

}