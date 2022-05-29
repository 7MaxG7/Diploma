using System;


namespace Infrastructure {

	internal interface IGameState {
		event Action OnStateEntered;
		void Exit();
	}

	internal interface IUnparamedGameState : IGameState {
		void Enter();

	}

	internal interface IParamedGameState<TParam> : IGameState {
		void Enter(TParam param);
	}
}