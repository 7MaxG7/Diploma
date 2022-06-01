using System;


namespace Infrastructure {

	internal interface IGameState {
		public event Action OnStateEntered;

		public void Exit();
	}

}