using System;


namespace Infrastructure {

	internal interface IMainMenuController : IDisposable {
		// event Action<string> OnPlayfabLogin;
		void SetupMainMenu();
		void OnDispose();
	}

}