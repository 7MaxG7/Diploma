using UI;
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
		
		public void Init(ICoroutineRunner coroutineRunner, PermanentUiView permanentUiView) {
			_permanentUiController.Init(coroutineRunner, permanentUiView);
			_sceneLoader.Init(coroutineRunner);
			
			_gameStateMachine.GetState(typeof(LoadMissionState)).OnStateEntered += EnterRunMissionState;
			_gameStateMachine.GetState(typeof(GameBootstrapState)).OnStateEntered += EnterLoadMissionState;
			_gameStateMachine.Enter<GameBootstrapState>();


			void EnterLoadMissionState() {
				_gameStateMachine.Enter<LoadMissionState, string>(TextConstants.MISSION_SCENE_NAME);
				_gameStateMachine.GetState(typeof(GameBootstrapState)).OnStateEntered -= EnterLoadMissionState;
			}

			void EnterRunMissionState() {
				_gameStateMachine.Enter<RunMissionState>();
				_gameStateMachine.GetState(typeof(LoadMissionState)).OnStateEntered -= EnterRunMissionState;
			}
		}
	}

}