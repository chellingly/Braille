using GPUTools.Common.Scripts.PL.Tools;

namespace GPUTools.Physics.Scripts.Types.Joints
{
    public struct GPDistanceJoint:IGroupItem
    {
        public int Body1Id;
        public int Body2Id;

        public float Distance;
        public float Elasticity;

        public GPDistanceJoint(int body1Id, int body2Id, float distance, float elasticity)
        {
            Body1Id = body1Id;
            Body2Id = body2Id;
            Distance = distance;
            Elasticity = elasticity;
        }

        public static int Size()
        {
            return sizeof(float)*2 + sizeof(int)*2;
        }

        public bool HasConflict(IGroupItem item)
        {
            var joint = (GPDistanceJoint) item;
            return joint.Body1Id == Body1Id
                   || joint.Body2Id == Body1Id
                   || joint.Body1Id == Body2Id
                   || joint.Body2Id == Body2Id;
        }
    }
}
