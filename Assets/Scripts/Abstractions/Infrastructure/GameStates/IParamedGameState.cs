namespace Infrastructure {

	internal interface IParamedGameState<TParam> : IGameState {
		void Enter(TParam param);
	}

}