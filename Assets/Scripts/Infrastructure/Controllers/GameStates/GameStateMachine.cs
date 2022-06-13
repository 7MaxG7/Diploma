using System;
using System.Collections.Generic;
using Zenject;


namespace Infrastructure {

	internal sealed class GameStateMachine : IGameStateMachine {
		private readonly Dictionary<Type, IGameState> _states;
		private IGameState _currentState;


		[Inject]
		public GameStateMachine(IGameBootstrapState gameBootstrapState, IMainMenuState mainMenuState, ILoadMissionState loadMissionState, IRunMissionState runMissionState) {
			_states = new Dictionary<Type, IGameState> {
					[typeof(GameBootstrapState)] = gameBootstrapState,
					[typeof(MainMenuState)] = mainMenuState,
					[typeof(LoadMissionState)] = loadMissionState,
					[typeof(RunMissionState)] = runMissionState
			};
		}

		public IGameState GetState(Type stateType) {
			return _states[stateType];
		}

		public void Enter<TState>() where TState : class, IUnparamedGameState {
			var newState = SwitchCurrentState<TState>();
			newState.Enter();
		}

		public void Enter<TState, TParam>(TParam param) where TState : class, IParamedGameState<TParam> {
			var newState = SwitchCurrentState<TState>();
			newState.Enter(param);
		}

		private TState SwitchCurrentState<TState>() where TState : class, IGameState {
			_currentState?.Exit();
			
			var newState = _states[typeof(TState)] as TState;
			_currentState = newState;
			
			return newState;
		}
	}

}