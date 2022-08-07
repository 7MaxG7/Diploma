using System;


namespace Infrastructure {

	internal interface IMissionResultController : IDisposable {
		event Action OnGameLeft;

		void Init();
	}

}