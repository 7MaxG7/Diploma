using System;


namespace Infrastructure {

	internal interface ISceneLoader {
		void Init(ICoroutineRunner coroutineRunner);
		void LoadScene(string sceneName, Action onSceneLoadedCallback = null);
		void LoadMissionScene(string sceneName, Action onSceneLoadedCallback);
	}

}