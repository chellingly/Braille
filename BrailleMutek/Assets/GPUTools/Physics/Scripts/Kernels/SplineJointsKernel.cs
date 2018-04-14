using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
    public class SplineJointsKernel : KernelBase
    {
        [GpuData("step")] public GpuValue<float> Step { set; get; }
        [GpuData("segments")] public GpuValue<int> Segments { set; get; }
        [GpuData("particles")] public GpuBuffer<GPParticle> Particles { set; get; }
        [GpuData("pointJoints")] public GpuBuffer<GPPointJoint> PointJoints { set; get; }
        [GpuData("transforms")] public GpuBuffer<Matrix4x4> Transforms { set; get; }

        public SplineJointsKernel() : base("Compute/SplineJoints", "CSSplineJoints")
        {

        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(Particles.Count / (float)GpuConfig.NumThreads);
        }
    }
}
