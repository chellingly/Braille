using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
    public static class ConvertUtils
    {
        public static Color ToColor(this Vector4 vector)
        {
            return new Color(vector.x, vector.y, vector.z, vector.w);
        }

        public static Vector4 ToVector(this Color color)
        {
            return new Color(color.r, color.g, color.b, color.a);
        }
    }
}
