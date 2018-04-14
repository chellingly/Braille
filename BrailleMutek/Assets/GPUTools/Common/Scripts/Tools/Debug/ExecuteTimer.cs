using System;

namespace GPUTools.Common.Scripts.Tools.Debug
{
    public class ExecuteTimer
    {
        public static DateTime StartTime;

        public static void Start()
        {
            StartTime = DateTime.Now;;
        }

        public static double TotalMiliseconds()
        {
            return (DateTime.Now - StartTime).TotalMilliseconds;
        }

        public static void Log()
        {
           UnityEngine.Debug.Log("Total Miliseconds: " + TotalMiliseconds());
        }
    }
}
