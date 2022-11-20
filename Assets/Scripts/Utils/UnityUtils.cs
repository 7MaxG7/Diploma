using UnityEditor;

namespace Utils
{
#if UNITY_EDITOR
    internal class UnityUtils
    {
        public static bool PlayModeIsStopping =>
            !EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying;
    }
#endif
}