using Utils;
using Zenject;


namespace Infrastructure {

	internal class Game : IGame {
		public IControllersHolder Controllers { get; private set; }
		
		private IGameStateMachine _gameStateMachine;
		private IPermanentUiController _permanentUiController;
		private ISceneLoader _sceneLoader;

		
		[Inject]
		private void InjectDependencies(IControllersHolder controllers, IGameStateMachine gameStateMachine, IPermanentUiController permanentUiController, ISceneLoader sceneLoader) {
			Controllers = controllers;
			_gameStateMachine = gameStateMachine;
			_permanentUiController = permanentUiController;
			_sceneLoader = sceneLoader;
		}
		
		public void Init(ICoroutineRunner coroutineRunner) {
			_permanentUiController.Init(coroutineRunner);
			_sceneLoader.Init(coroutineRunner);
			
			_gameStateMachine.GetState(typeof(LoadMissionState)).OnStateChange += EnterRunMissionState;
			_gameStateMachine.GetState(typeof(MainMenuState)).OnStateChange += EnterLoadMissionState;
			_gameStateMachine.GetState(typeof(GameBootstrapState)).OnStateChange += EnterMainMenuState;
			_gameStateMachine.Enter<GameBootstrapState>();


			void EnterMainMenuState() {
				_gameStateMachine.Enter<MainMenuState>();
				_gameStateMachine.GetState(typeof(GameBootstrapState)).OnStateChange -= EnterMainMenuState;
			}

			void EnterLoadMissionState() {
				_gameStateMachine.Enter<LoadMissionState, string>(TextConstants.MISSION_SCENE_NAME);
				_gameStateMachine.GetState(typeof(MainMenuState)).OnStateChange -= EnterLoadMissionState;
			}

			void EnterRunMissionState() {
				_gameStateMachine.Enter<RunMissionState>();
				_gameStateMachine.GetState(typeof(LoadMissionState)).OnStateChange -= EnterRunMissionState;
			}
		}
	}

}