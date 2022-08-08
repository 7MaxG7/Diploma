using System;


namespace Infrastructure {

	internal interface IGameStateMachine : IDisposable {
		IGameState GetState(Type stateType);

		void Enter<TState>() where TState : class, IUnparamedGameState;

		void Enter<TState, TParam>(TParam param) where TState : class, IParamedGameState<TParam>;
	}

}