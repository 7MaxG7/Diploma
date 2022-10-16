using UnityEngine;

namespace Services
{
    internal interface IPlayerPrefsService
    {
        Resolution? GetSavedResolution();
        bool? GetFullScreenMode();
        (float MusicVolume, float SoundVolume)? GetSoundVolumes();
        void SaveCurrentResolution(Resolution resolution);
        void SaveSoundVolumes(float musicVolume, float soundVolume);
        void SaveCurrentFullScreenMode(bool isFullScreen);
    }
}