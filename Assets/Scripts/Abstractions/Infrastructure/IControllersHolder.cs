namespace Infrastructure {

	internal interface IControllersHolder : IUpdater, ILateUpdater, IFixedUpdater, IDisposer {
		void AddController(IController controller);
		void ClearControllers();
	}

}