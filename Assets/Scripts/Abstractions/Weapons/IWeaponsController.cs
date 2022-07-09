using Services;
using Units;


namespace Infrastructure {

	internal interface IWeaponsController : IUpdater {
		void Init(IUnit player);
		void StopShooting();
		void StartShooting();
		void AddWeapon(WeaponType type);
	}

}