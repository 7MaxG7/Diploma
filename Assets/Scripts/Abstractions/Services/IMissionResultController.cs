using System;
using Units;


namespace Infrastructure {

	internal interface IMissionResultController : IDisposable {
		event Action OnGameLeft;

		void Init(IUnit player);
		void CountDead(DamageInfo damageInfo);
	}

}