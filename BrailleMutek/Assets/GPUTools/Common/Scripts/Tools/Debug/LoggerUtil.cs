using System;
using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Tools.Debug
{
    public class LoggerUtil
    {
        public static void LogArray<T>(T[] list, int max)
        {
            for (var i = 0; i < Math.Min(list.Length, max); i++)
            {
                UnityEngine.Debug.Log(list[i]);
            }
        }

        public static void LogList<T>(List<T> list, int max)
        {
            for (var i = 0; i < Math.Min(list.Count, max); i++)
            {
                UnityEngine.Debug.Log(list[i]);
            }
        }

    }
}
