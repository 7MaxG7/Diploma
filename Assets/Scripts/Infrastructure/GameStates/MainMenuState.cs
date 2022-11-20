using System;
using System.Threading.Tasks;
using Services;
using Sounds;
using UI;
using UnityEngine.SceneManagement;
using Utils;
using Zenject;


namespace Infrastructure
{
    internal sealed class MainMenuState : IMainMenuState
    {
        public event Action OnStateChange;

        private readonly IMainMenuController _mainMenuController;
        private readonly IPermanentUiController _permanentUiController;
        private readonly ISoundController _soundController;
        private readonly IAssetProvider _assetProvider;
        private readonly ISceneLoader _sceneLoader;


        [Inject]
        public MainMenuState(ISceneLoader sceneLoader, IMainMenuController mainMenuController,
            IPermanentUiController permanentUiController, ISoundController soundController, IAssetProvider assetProvider)
        {
            _sceneLoader = sceneLoader;
            _mainMenuController = mainMenuController;
            _permanentUiController = permanentUiController;
            _soundController = soundController;
            _assetProvider = assetProvider;
        }

        public void Enter()
        {
            _assetProvider.WarmUpForState(GetType());
            _soundController.PlayRandomMenuMusic();
            _sceneLoader.LoadScene(Constants.MAIN_MENU_SCENE_NAME, SetupMainMenu);
        }

        public void Exit()
        {
            _soundController.StopAll();
            if (!_permanentUiController.IsActivating)
                OnMainMenuDispose();
            else
                _permanentUiController.OnCurtainShown += DisposeMainMenu;


            void DisposeMainMenu()
            {
                _permanentUiController.OnCurtainShown -= DisposeMainMenu;
                OnMainMenuDispose();
            }
        }

        private async void SetupMainMenu()
        {
            await _mainMenuController.SetupMainMenu();
            SceneManager.sceneUnloaded += ShowCurtain;
            SceneManager.sceneLoaded += SwitchState;

            _permanentUiController.HideLoadingCurtain(interruptCurrentAnimation: true);
        }

        private void ShowCurtain(Scene scene)
        {
            SceneManager.sceneUnloaded -= ShowCurtain;
            if (!_permanentUiController.CurtainIsActive)
                _permanentUiController.ShowLoadingCurtain(false);
        }

        private void SwitchState(Scene scene, LoadSceneMode sceneMode)
        {
            SceneManager.sceneLoaded -= SwitchState;
            OnStateChange?.Invoke();
        }

        private void OnMainMenuDispose()
        {
            _mainMenuController.OnDispose();
            _assetProvider.CleanUp();
        }
    }
}