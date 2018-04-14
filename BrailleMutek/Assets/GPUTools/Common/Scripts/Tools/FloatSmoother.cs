using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
    public class FloatSmoother
    {
        private float[] buffer;

        public FloatSmoother(int bufferLength)
        {
            buffer = new float[bufferLength];

            for (int i = 0; i < bufferLength; i++)
            {
                buffer[i] = Time.fixedDeltaTime;
            }
        }

        public void AddValue(float value)
        {
            for (int i = 0; i < buffer.Length - 1; i++)
            {
                buffer[i] = buffer[i + 1];
            }

            buffer[buffer.Length - 1] = value;

        }

        public float GetSmoothedValue()
        {
            var result = 0f;

            for (var i = 0; i < buffer.Length; i++)
            {
                result += buffer[i];
            }

            return result / buffer.Length;
        }
    }
}
