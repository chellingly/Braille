using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
    public class ResetKernel : KernelBase
    {
        [GpuData("particles")]
        public GpuBuffer<GPParticle> Particles { set; get; }

        public ResetKernel() : base("Compute/Reset", "CSReset")
        {

        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(Particles.ComputeBuffer.count / (float)GpuConfig.NumThreads);
        }
    }
}
