using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Tools
{
    public static class ComputeUtils
    {
        public static ComputeBuffer ToComputeBuffer<T>(this T[] array, int stride,
            ComputeBufferType type = ComputeBufferType.Default)
        {
            var buffer = new ComputeBuffer(array.Length, stride, type);
            buffer.SetData(array);
            return buffer;
        }

        public static T[] ToArray<T>(this ComputeBuffer buffer)
        {
            var array = new T[buffer.count];
            buffer.GetData(array);
            return array;
        }

        public static void LogBuffer<T>(ComputeBuffer buffer)
        {
            var array = new T[buffer.count];
            buffer.GetData(array);

            for (var i = 0; i < array.Length; i++)
                Debug.Log(string.Format("i:{0} val:{1}", i, array[i]));
        }

        public static void LogLargeBuffer<T>(ComputeBuffer buffer)
        {
            var array = new T[buffer.count];
            buffer.GetData(array);

            var log = "";
            for (var i = 1; i <= array.Length; i++)
            {
                log += "|" + array[i - 1];

                if (i % 12 == 0)
                {
                    Debug.Log(string.Format("from i:{0} values:{1}", i, log));
                    log = "";
                } 
            }
        }
    }
}