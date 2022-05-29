namespace Infrastructure {

	internal interface IUpdater : IController {
		void OnUpdate(float deltaTime);
	}

}