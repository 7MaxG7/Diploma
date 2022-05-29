namespace Infrastructure {

	internal interface ILateUpdater : IController {
		void OnLateUpdate(float deltaTime);
	}

}