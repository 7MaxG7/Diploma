namespace Infrastructure
{
    internal interface IFixedUpdater : IController
    {
        public void OnFixedUpdate(float deltaTime);
    }
}