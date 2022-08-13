using System;
using Units;


namespace Infrastructure {

	internal interface IMissionResultController : IDisposable {
		event Action OnGameLeft;
		event Action OnPlayerLeftRoomEvent;

		void Init(IUnit player);
		void CountDead(DamageInfo damageInfo);
	}

}