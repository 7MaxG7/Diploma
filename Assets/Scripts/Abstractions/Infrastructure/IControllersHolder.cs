namespace Infrastructure {

	internal interface IControllersHolder : IUpdater, ILateUpdater, IFixedUpdater {
		void AddController(IController controller);
		void ClearControllers();
	}

}