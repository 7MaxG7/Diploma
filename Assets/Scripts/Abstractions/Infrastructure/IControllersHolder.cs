namespace Infrastructure
{
    internal interface IControllersHolder : IUpdater, ILateUpdater, IFixedUpdater
    {
        void AddController(IController controller);
        void RemoveController(IController controller);
        void ClearControllers();
    }
}