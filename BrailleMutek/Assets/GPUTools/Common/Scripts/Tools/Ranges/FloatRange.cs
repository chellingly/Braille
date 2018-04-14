using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Ranges
{
    [Serializable]
    public struct FloatRange
    {
        public float Min;
        public float Max;

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float GetRandom()
        {
            return UnityEngine.Random.Range(Min, Max);
        }

        public float GetLerp(float t)
        {
            return Mathf.Lerp(Min, Max, t);
        }
    }
}
