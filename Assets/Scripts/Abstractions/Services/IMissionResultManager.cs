using System;
using Units;


namespace Infrastructure {

	internal interface IMissionResultManager : IDisposable {
		event Action OnGameLeft;
		event Action<int> OnPlayerWithIdLeftRoomEvent;

		void Init(IUnit player);
		void CountKill(DamageInfo damageInfo);
		void SetWinsAmount(int winsAmount);
		void SetKillsAmount(int killsAmount);
	}

}