using System;
using UnityEngine.SceneManagement;
using Utils;
using Zenject;


namespace Infrastructure {

	internal sealed class MainMenuState : IMainMenuState {
		public event Action OnStateChange;
		
		private readonly IMainMenuController _mainMenuController;
		private readonly IPermanentUiController _permanentUiController;
		private readonly ISoundController _soundController;
		private readonly ISceneLoader _sceneLoader;


		[Inject]
		public MainMenuState(ISceneLoader sceneLoader, IMainMenuController mainMenuController, IPermanentUiController permanentUiController
				, ISoundController soundController) {
			_sceneLoader = sceneLoader;
			_mainMenuController = mainMenuController;
			_permanentUiController = permanentUiController;
			_soundController = soundController;
		}

		public void Enter() {
			_soundController.PlayRandomMenuMusic();
			_sceneLoader.LoadScene(Constants.MAIN_MENU_SCENE_NAME, SetupMainMenu);
		}

		private void SetupMainMenu() {
			_mainMenuController.SetupMainMenu();
			SceneManager.sceneUnloaded += ShowCurtain;
			SceneManager.sceneLoaded += SwitchState;
			
			_permanentUiController.HideLoadingCurtain(interruptCurrentAnimation: true);
		}

		private void ShowCurtain(Scene scene) {
			SceneManager.sceneUnloaded -= ShowCurtain;
			if (!_permanentUiController.CurtainIsActive)
				_permanentUiController.ShowLoadingCurtain(false);
		}

		private void SwitchState(Scene scene, LoadSceneMode sceneMode) {
			SceneManager.sceneLoaded -= SwitchState;
			OnStateChange?.Invoke();
		}

		public void Exit() {
			_soundController.StopAll();
			if (!_permanentUiController.IsActivating)
				_mainMenuController.Dispose();
			else
				_permanentUiController.OnCurtainShown += DisposeMainMenu;


			void DisposeMainMenu() {
				_permanentUiController.OnCurtainShown -= DisposeMainMenu;
				_mainMenuController.Dispose();
			}
		}
	}

}