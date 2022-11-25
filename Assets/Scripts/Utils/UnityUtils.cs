using System;
using UnityEditor;

namespace Utils
{
#if UNITY_EDITOR
     [InitializeOnLoad]
     internal class UnityUtils
    {
        public static event Action OnPlayModeExit;

        public static bool PlayModeIsStopping =>
            !EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying;


        static UnityUtils()
        {
            EditorApplication.playModeStateChanged += InvokePlayModeStopActions;
        }

        ~UnityUtils()
        {
            EditorApplication.playModeStateChanged -= InvokePlayModeStopActions;
        }

        private static void InvokePlayModeStopActions(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingPlayMode:
                    OnPlayModeExit?.Invoke();
                    break;
            }
        }
    }
#endif
}