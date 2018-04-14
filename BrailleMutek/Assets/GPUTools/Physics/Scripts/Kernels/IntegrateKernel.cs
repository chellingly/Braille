using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
    public class IntegrateKernel : KernelBase
    {
        [GpuData("particles")]
        public GpuBuffer<GPParticle> Particles { set; get; }

        [GpuData("gravity")]
        public GpuValue<Vector3> Gravity { set; get; }

        [GpuData("invDrag")]
        public GpuValue<float> InvDrag { set; get; }

        [GpuData("dt")] 
        public GpuValue<float> DT { set; get; }
        
        [GpuData("wind")]
        public GpuValue<Vector3> Wind { set; get; }

        public IntegrateKernel() : base("Compute/Integrate", "CSIntegrate")
        {
        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(Particles.ComputeBuffer.count/ (float)GpuConfig.NumThreads);
        }
    }
}
