using System;
using System.Threading.Tasks;

namespace Infrastructure
{
    internal interface IMainMenuController : IDisposable
    {
        Task SetupMainMenu();
        void OnDispose();
    }
}