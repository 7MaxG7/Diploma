using System;
using Units;


namespace Infrastructure {

	internal interface IMissionResultController : IDisposable {
		event Action OnGameLeft;
		event Action OnPlayerLeftRoomEvent;

		void Init(IUnit player);
		void CountKill(DamageInfo damageInfo);
		void SetWinsAmount(int winsAmount);
		void SetKillsAmount(int killsAmount);
	}

}