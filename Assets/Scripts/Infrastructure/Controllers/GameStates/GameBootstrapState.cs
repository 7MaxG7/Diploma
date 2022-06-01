using System;
using Services;
using Utils;
using Zenject;


namespace Infrastructure {

	internal class GameBootstrapState : IGameBootstrapState {
		public event Action OnStateEntered;
		
		private readonly ISceneLoader _sceneLoader;
		private readonly IPermanentUiController _permanentUiController;
		private readonly IInputService _inputService;


		[Inject]
		public GameBootstrapState(ISceneLoader sceneLoader, IPermanentUiController permanentUiController, IInputService inputService) {
			_sceneLoader = sceneLoader;
			_permanentUiController = permanentUiController;
			_inputService = inputService;
		}
		
		public void Enter() {
			_permanentUiController.ShowLoadingCurtain(false);
			_inputService.Init();
			_sceneLoader.LoadScene(TextConstants.BOOTSTRAP_SCENE_NAME, () => OnStateEntered?.Invoke());
		}

		public void Exit() {
		}
	}

}