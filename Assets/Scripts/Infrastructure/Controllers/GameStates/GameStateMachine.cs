using System;
using System.Collections.Generic;


namespace Infrastructure {

	internal sealed class GameStateMachine {
		private readonly Dictionary<Type, IGameState> _states;
		private IGameState _currentState;
		

		public GameStateMachine(ControllersBox controllers, SceneLoader sceneLoader, PermanentUiController permanentUiController) {
			_states = new Dictionary<Type, IGameState> {
					[typeof(GameBootstrapState)] = new GameBootstrapState(sceneLoader, permanentUiController),
					[typeof(LoadMissionState)] = new LoadMissionState(sceneLoader, controllers, permanentUiController),
					[typeof(RunMissionState)] = new RunMissionState()
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