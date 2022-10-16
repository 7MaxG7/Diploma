using UnityEngine;

namespace Services
{
    internal interface IScreenService
    {
        Resolution GetCurrentResolution();
        bool GetCurrentFullScreenMode();
    }
}