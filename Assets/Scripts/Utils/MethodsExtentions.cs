using UnityEngine;

namespace Utils
{
    public static class MethodsExtentions
    {
        public static bool Equals(this Resolution resolution, Resolution otherResolution)
        {
            return resolution.width == otherResolution.width && resolution.height == otherResolution.height
                                                             && resolution.refreshRate == otherResolution.refreshRate;
        }
    }
}