using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Utils
{
    public static class CollectionUtils
    {
        public static bool NullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static bool NullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        public static void SetValueForAll<T>(this T[] array, T value)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
    }
}
