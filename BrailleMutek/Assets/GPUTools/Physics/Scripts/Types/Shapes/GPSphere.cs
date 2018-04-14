using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
    public struct GPSphere
    {
        public Vector3 Position;
        public float Radius;

        public GPSphere(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public static int Size()
        {
            return sizeof (float)*4;
        }
    }
}
