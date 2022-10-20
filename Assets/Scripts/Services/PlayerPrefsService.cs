using UnityEngine;
using Utils;

namespace Services
{
    internal class PlayerPrefsService : IPlayerPrefsService
    {
        #region GetPrefs

        public Resolution? GetSavedResolution()
        {
            var width = PlayerPrefs.GetInt(Constants.SAVED_WIDTH_PREFS_KEY, 0);
            var height = PlayerPrefs.GetInt(Constants.SAVED_HEIGHT_PREFS_KEY, 0);
            var refreshRate = PlayerPrefs.GetInt(Constants.SAVED_FRAMERATE_PREFS_KEY, 0);
            if (width == 0 || height == 0 || refreshRate == 0)
                return null;
            return new Resolution { width = width, height = height, refreshRate = refreshRate };
        }

        public bool? GetFullScreenMode()
        {
            var fullScreen = PlayerPrefs.GetInt(Constants.SAVED_WINDOW_MODE_PREFS_KEY, -1);
            if (fullScreen == -1)
                return null;
            return fullScreen != 0;
        }

        public (float MusicVolume, float SoundVolume)? GetSoundVolumes()
        {
            var musicVolume = PlayerPrefs.GetFloat(Constants.MUSIC_VOLUME_PREFS_KEY, -1);
            var soundVolume = PlayerPrefs.GetFloat(Constants.SOUND_VOLUME_PREFS_KEY, -1);
            if (musicVolume < 0 || soundVolume < 0)
                return null;
            return (musicVolume, soundVolume);
        }

        #endregion

        #region SetPrefs

        public void SaveCurrentResolution(Resolution resolution)
        {
            PlayerPrefs.SetInt(Constants.SAVED_WIDTH_PREFS_KEY, resolution.width);
            PlayerPrefs.SetInt(Constants.SAVED_HEIGHT_PREFS_KEY, resolution.height);
            PlayerPrefs.SetInt(Constants.SAVED_FRAMERATE_PREFS_KEY, resolution.refreshRate);
            PlayerPrefs.Save();
        }

        public void SaveCurrentFullScreenMode(bool isFullScreen)
        {
            PlayerPrefs.SetInt(Constants.SAVED_WINDOW_MODE_PREFS_KEY, isFullScreen ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void SaveSoundVolumes(float musicVolume, float soundVolume)
        {
            PlayerPrefs.SetFloat(Constants.MUSIC_VOLUME_PREFS_KEY, musicVolume);
            PlayerPrefs.SetFloat(Constants.SOUND_VOLUME_PREFS_KEY, soundVolume);
            PlayerPrefs.Save();
        }

        #endregion
    }
}