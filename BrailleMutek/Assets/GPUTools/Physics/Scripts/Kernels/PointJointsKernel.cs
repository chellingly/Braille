using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
    public class PointJointsKernel:KernelBase
    {
        [GpuData("step")]
        public GpuValue<float> Step { set; get; }

        [GpuData("t")] public GpuValue<float> T { set; get; }

        [GpuData("isFixed")] public GpuValue<int> IsFixed { set; get; }

        [GpuData("pointJoints")]
        public GpuBuffer<GPPointJoint> PointJoints { set; get; }

        [GpuData("particles")]
        public GpuBuffer<GPParticle> Particles { set; get; }

        [GpuData("transforms")]
        public GpuBuffer<Matrix4x4> Transforms { set; get; }

        [GpuData("oldTransforms")]
        public GpuBuffer<Matrix4x4> OldTransforms { set; get; }

        public PointJointsKernel() : base("Compute/PointJoints", "CSPointJoints")
        {
        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(PointJoints.ComputeBuffer.count / (float)GpuConfig.NumThreads);
        }
    }
}
