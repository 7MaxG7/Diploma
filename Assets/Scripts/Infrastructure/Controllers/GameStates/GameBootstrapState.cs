using System;
using Services;
using UnityEngine;
using Utils;


namespace Infrastructure {

	internal class GameBootstrapState : IUnparamedGameState {
		public event Action OnStateEntered;
		
		private readonly SceneLoader _sceneLoader;
		private readonly PermanentUiController _permanentUiController;


		public GameBootstrapState(SceneLoader sceneLoader, PermanentUiController permanentUiController) {
			_sceneLoader = sceneLoader;
			_permanentUiController = permanentUiController;
		}
		
		public void Enter() {
			_permanentUiController.ShowLoadingCurtain(false);
			if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
				Game.InputService = new PcInputService();
			else
				Game.InputService = new MobileInputService();
			
			Game.InputService.Init();

			_sceneLoader.LoadScene(TextConstants.BOOTSTRAP_SCENE_NAME, () => OnStateEntered?.Invoke());
		}

		public void Exit() {
			
		}
	}

}