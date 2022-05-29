using UnityEngine;


namespace Utils {

	internal class UnitsFactory : IUnitsFactory {
		public CharacterController CreatePlayer() {
			return Object.Instantiate(Resources.Load<CharacterController>(TextConstants.PLAYER_PREF_RESOURCES_PATH));
		}
	}

}