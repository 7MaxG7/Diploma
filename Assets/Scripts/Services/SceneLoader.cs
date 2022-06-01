using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Infrastructure {

	internal sealed class SceneLoader : ISceneLoader {
		private ICoroutineRunner _coroutineRunner;

		// public SceneLoader(ICoroutineRunner coroutineRunner) {
		// 	_coroutineRunner = coroutineRunner;
		// }

		public void Init(ICoroutineRunner coroutineRunner) {
			_coroutineRunner = coroutineRunner;
		}

		public void LoadScene(string sceneName, Action onSceneLoadedCallback = null) {
			_coroutineRunner.StartCoroutine(LoadSceneCoroutine(sceneName, onSceneLoadedCallback));
		}

		private IEnumerator LoadSceneCoroutine(string sceneName, Action onSceneLoadedCallback) {
			if (SceneManager.GetActiveScene().name == sceneName) {
				onSceneLoadedCallback?.Invoke();
				yield break;
			}
			
			var loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
			while (!loadSceneOperation.isDone)
				yield return new WaitForEndOfFrame();

			onSceneLoadedCallback?.Invoke();
		}
	}

}