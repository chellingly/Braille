using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Joints
{
    public struct GPPointJoint
    {
        public int BodyId;
        public int MatrixId;
        public Vector3 Point;
        public float Elasticity;

        public GPPointJoint(int bodyId,  int matrixId, Vector3 point, float elasticity)
        {
            BodyId = bodyId;
            Point = point;
            MatrixId = matrixId;
            Elasticity = elasticity;
        }

        public static int Size()
        {
            return sizeof(int)*2 + sizeof(float)*3 + sizeof(float);
        }
    }
}
