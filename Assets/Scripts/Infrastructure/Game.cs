using System;
using Services;
using Sounds;
using UI;
using Utils;
using Zenject;


namespace Infrastructure
{
    internal sealed class Game : IGame, IDisposable
    {
        public IControllersHolder Controllers { get; private set; }

        private IGameStateMachine _gameStateMachine;
        private IPermanentUiController _permanentUiController;
        private ISceneLoader _sceneLoader;
        private ISoundController _soundController;
        private IAssetProvider _assetProvider;


        [Inject]
        private void InjectDependencies(IControllersHolder controllers, IGameStateMachine gameStateMachine
            , ISoundController soundController, IPermanentUiController permanentUiController
            , ISceneLoader sceneLoader, IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _soundController = soundController;
            Controllers = controllers;
            _gameStateMachine = gameStateMachine;
            _permanentUiController = permanentUiController;
            _sceneLoader = sceneLoader;
        }

        public void Dispose()
        {
            _gameStateMachine.GetState(typeof(GameBootstrapState)).OnStateChange -= EnterMainMenuState;
            _gameStateMachine.GetState(typeof(MainMenuState)).OnStateChange -= EnterLoadMissionState;
            _gameStateMachine.GetState(typeof(LoadMissionState)).OnStateChange -= EnterRunMissionState;
            _gameStateMachine.GetState(typeof(RunMissionState)).OnStateChange -= EnterLeaveMissionState;
            _gameStateMachine.GetState(typeof(LeaveMissionState)).OnStateChange -= EnterMainMenuState;
        }

        public void Init(ICoroutineRunner coroutineRunner)
        {
            _assetProvider.Init();
            _soundController.Init();
            _permanentUiController.Init(coroutineRunner);
            _sceneLoader.Init(coroutineRunner);

            _gameStateMachine.GetState(typeof(LeaveMissionState)).OnStateChange += EnterMainMenuState;
            _gameStateMachine.GetState(typeof(RunMissionState)).OnStateChange += EnterLeaveMissionState;
            _gameStateMachine.GetState(typeof(LoadMissionState)).OnStateChange += EnterRunMissionState;
            _gameStateMachine.GetState(typeof(MainMenuState)).OnStateChange += EnterLoadMissionState;
            _gameStateMachine.GetState(typeof(GameBootstrapState)).OnStateChange += EnterMainMenuState;
            _gameStateMachine.Enter<GameBootstrapState>();
        }

        private void EnterMainMenuState()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void EnterLoadMissionState()
        {
            _gameStateMachine.Enter<LoadMissionState, string>(Constants.MISSION_SCENE_NAME);
        }

        private void EnterRunMissionState()
        {
            _gameStateMachine.Enter<RunMissionState>();
        }

        private void EnterLeaveMissionState()
        {
            _gameStateMachine.Enter<LeaveMissionState>();
        }
    }
}