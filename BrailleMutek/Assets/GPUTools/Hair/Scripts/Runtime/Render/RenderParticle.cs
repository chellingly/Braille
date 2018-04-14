using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Render
{
    public struct RenderParticle
    {
        public Vector3 Color;
        public float Interpolation;
        public float WavinessScale;
        public float WavinessFrequency;
        
        public static int Size()
        {
            return sizeof(float) * 6;
        }
    }
}
