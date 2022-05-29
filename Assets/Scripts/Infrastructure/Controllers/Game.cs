using Services;
using UI;


namespace Infrastructure {

	internal class Game {
		public ControllersBox Controllers { get; }
		
		public static IInputService InputService;
		public readonly GameStateMachine GameStateMachine;


		public Game(ICoroutineRunner coroutineRunner, PermanentUiView permanentUiView) {
			Controllers = new ControllersBox();
			var permanentUiController = new PermanentUiController(permanentUiView, coroutineRunner);
			GameStateMachine = new GameStateMachine(Controllers, new SceneLoader(coroutineRunner), permanentUiController);
		}

	}

}