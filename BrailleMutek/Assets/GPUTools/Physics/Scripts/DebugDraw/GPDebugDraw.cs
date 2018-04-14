using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.DebugDraw
{
    public class GPDebugDraw
    {
        public static void Draw(GpuBuffer<GPDistanceJoint> joints, GpuBuffer<GPParticle> particles, bool drawParticles, bool drawJoints)
        {
            particles.PullData();

            if(drawJoints)
                joints.PullData();

            Gizmos.color = Color.green;

            if (drawParticles)
            {
                foreach (var particle in particles.Data)
                {
                    Gizmos.DrawWireSphere(particle.Position, particle.Radius);
                }
            }

            if (drawJoints)
            {
                foreach (var joint in joints.Data)
                {
                    var p1 = particles.Data[joint.Body1Id];
                    var p2 = particles.Data[joint.Body2Id];

                    Gizmos.DrawLine(p1.Position, p2.Position);
                }
            }
        }
    }
}
