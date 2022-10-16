using UnityEngine;

namespace Services
{
    internal class ScreenService : IScreenService
    {
        public Resolution GetCurrentResolution()
        {
            return Screen.fullScreen
                ? Screen.currentResolution
                // Despite of documentation for current version in window mode Screen.currentResolution returnes desktop but game resolution
                : new Resolution { width = Screen.width, height = Screen.height, refreshRate = Screen.currentResolution.refreshRate };
        }

        public bool GetCurrentFullScreenMode()
        {
            return Screen.fullScreen;
        }
    }
}