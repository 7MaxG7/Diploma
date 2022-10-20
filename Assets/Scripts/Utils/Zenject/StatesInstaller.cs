using Zenject;


namespace Infrastructure.Zenject
{
    internal sealed class StatesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameBootstrapState>().To<GameBootstrapState>().AsSingle();
            Container.Bind<IMainMenuState>().To<MainMenuState>().AsSingle();
            Container.Bind<ILoadMissionState>().To<LoadMissionState>().AsSingle();
            Container.Bind<IRunMissionState>().To<RunMissionState>().AsSingle();
            Container.Bind<ILeaveMissionState>().To<LeaveMissionState>().AsSingle();

            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
        }
    }
}