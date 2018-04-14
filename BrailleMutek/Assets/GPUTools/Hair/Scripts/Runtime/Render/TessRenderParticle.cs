using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Render
{
    public struct TessRenderParticle
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Tangent;
        public Vector3 Color;
        public float Interpolation;

        public static int Size()
        {
            return sizeof(float) * 13;
        }
    }
}
