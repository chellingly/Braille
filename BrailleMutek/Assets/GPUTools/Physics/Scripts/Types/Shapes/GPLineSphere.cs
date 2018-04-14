using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
    public struct GPLineSphere
    {
        public Vector3 PositionA;
        public Vector3 PositionB;

        public float RadiusA;
        public float RadiusB;

        public GPLineSphere(Vector3 positionA, Vector3 positionB, float radiusA, float radiusB)
        {
            PositionA = positionA;
            PositionB = positionB;
            RadiusA = radiusA;
            RadiusB = radiusB;
        }

        public static int Size()
        {
            return sizeof (float)*8;
        }
    }
}
