using Units;


namespace Infrastructure {

	internal interface IWeapon {
		float SqrRange { get; }
		bool IsReady { get; }
		WeaponType Type { get; }

		void ReduceCooldown(float deltaTime);
		void Shoot(IUnit target);
	}

}