using System;
using System.Collections;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;


namespace Infrastructure
{
    internal sealed class SceneLoader : ISceneLoader
    {
        private readonly IPhotonManager _photonManager;
        private ICoroutineRunner _coroutineRunner;


        public SceneLoader(IPhotonManager photonManager)
        {
            _photonManager = photonManager;
        }

        public void Init(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadScene(string sceneName, Action onSceneLoadedCallback = null)
        {
            _coroutineRunner.StartCoroutine(LoadSceneCoroutine(sceneName, onSceneLoadedCallback));
        }

        public void LoadMissionScene(string sceneName, Action onSceneLoadedCallback)
        {
            _coroutineRunner.StartCoroutine(LoadMissionSceneCoroutine(sceneName, onSceneLoadedCallback));
        }

        public static bool SceneIsMission(Scene scene)
        {
            return scene.name == Constants.MISSION_SCENE_NAME;
        }

        private IEnumerator LoadMissionSceneCoroutine(string sceneName, Action onSceneLoadedCallback)
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == sceneName)
            {
                onSceneLoadedCallback?.Invoke();
                yield break;
            }

            _photonManager.LoadLevel(sceneName);
            while (_photonManager.GetLevelLoadingProgress() < 1)
                yield return new WaitForEndOfFrame();

            onSceneLoadedCallback?.Invoke();
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, Action onSceneLoadedCallback)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
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