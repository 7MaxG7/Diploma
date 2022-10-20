namespace Infrastructure
{
    internal interface ILateUpdater : IController
    {
        public void OnLateUpdate(float deltaTime);
    }
}