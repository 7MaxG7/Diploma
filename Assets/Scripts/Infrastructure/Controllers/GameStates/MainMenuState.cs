using System;
using UnityEngine.SceneManagement;
using Utils;
using Zenject;


namespace Infrastructure {

	internal class MainMenuState : IMainMenuState {
		public event Action OnStateChange;
		
		private readonly IMainMenuController _mainMenuController;
		private readonly IPermanentUiController _permanentUiController;
		private readonly ISceneLoader _sceneLoader;
		private readonly IMainMenuView _mainMenuView;


		[Inject]
		public MainMenuState(ISceneLoader sceneLoader, IMainMenuController mainMenuController, IPermanentUiController permanentUiController) {
			_sceneLoader = sceneLoader;
			_mainMenuController = mainMenuController;
			_permanentUiController = permanentUiController;
		}

		public void Enter() {
			_permanentUiController.HideLoadingCurtain();
			_sceneLoader.LoadScene(TextConstants.MAIN_MENU_SCENE_NAME, SetupMainMenu);
		}

		private void SetupMainMenu() {
			_mainMenuController.SetupMainMenu();
			SceneManager.sceneUnloaded += ShowCurtain;
			SceneManager.sceneLoaded += SwitchState;
		}

		private void ShowCurtain(Scene scene) {
			SceneManager.sceneUnloaded -= ShowCurtain;
			_permanentUiController.ShowLoadingCurtain(isForced: true);
		}

		private void SwitchState(Scene scene, LoadSceneMode sceneMode) {
			SceneManager.sceneLoaded -= SwitchState;
			OnStateChange?.Invoke();
		}

		public void Exit() {
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