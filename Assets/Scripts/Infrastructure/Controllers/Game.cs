using System;
using Utils;
using Zenject;


namespace Infrastructure {

	internal class Game : IGame, IDisposable {
		public IControllersHolder Controllers { get; private set; }
		
		private IGameStateMachine _gameStateMachine;
		private IPermanentUiController _permanentUiController;
		private ISceneLoader _sceneLoader;
		private ISoundManager _soundManager;


		[Inject]
		private void InjectDependencies(IControllersHolder controllers, IGameStateMachine gameStateMachine, ISoundManager soundManager
				, IPermanentUiController permanentUiController, ISceneLoader sceneLoader) {
			_soundManager = soundManager;
			Controllers = controllers;
			_gameStateMachine = gameStateMachine;
			_permanentUiController = permanentUiController;
			_sceneLoader = sceneLoader;
		}

		public void Dispose() {
			_gameStateMachine.GetState(typeof(GameBootstrapState)).OnStateChange -= EnterMainMenuState;
			_gameStateMachine.GetState(typeof(MainMenuState)).OnStateChange -= EnterLoadMissionState;
			_gameStateMachine.GetState(typeof(LoadMissionState)).OnStateChange -= EnterRunMissionState;
			_gameStateMachine.GetState(typeof(RunMissionState)).OnStateChange -= EnterLeaveMissionState;
			_gameStateMachine.GetState(typeof(LeaveMissionState)).OnStateChange -= EnterMainMenuState;
		}

		public void Init(ICoroutineRunner coroutineRunner) {
			_soundManager.Init();
			_permanentUiController.Init(coroutineRunner);
			_sceneLoader.Init(coroutineRunner);
			
			_gameStateMachine.GetState(typeof(LeaveMissionState)).OnStateChange += EnterMainMenuState;
			_gameStateMachine.GetState(typeof(RunMissionState)).OnStateChange += EnterLeaveMissionState;
			_gameStateMachine.GetState(typeof(LoadMissionState)).OnStateChange += EnterRunMissionState;
			_gameStateMachine.GetState(typeof(MainMenuState)).OnStateChange += EnterLoadMissionState;
			_gameStateMachine.GetState(typeof(GameBootstrapState)).OnStateChange += EnterMainMenuState;
			_gameStateMachine.Enter<GameBootstrapState>();
		}

		private void EnterMainMenuState() {
			_gameStateMachine.Enter<MainMenuState>();
		}

		private void EnterLoadMissionState() {
			_gameStateMachine.Enter<LoadMissionState, string>(TextConstants.MISSION_SCENE_NAME);
		}

		private void EnterRunMissionState() {
			_gameStateMachine.Enter<RunMissionState>();
		}

		private void EnterLeaveMissionState() {
			_gameStateMachine.Enter<LeaveMissionState>();
		}

	}

}