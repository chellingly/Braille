using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Dynamic
{
    public struct  GPParticle
    {
        public Vector3 Position;
        public Vector3 LastPosition;
        public float Radius;
        
        public GPParticle(Vector3 position, float radius)
        {
            Position = position;
            LastPosition = position;
            Radius = radius;
        }

        public static int Size()
        {
            return sizeof (float)*7;
        }
    }
}
