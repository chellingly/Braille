using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;

namespace GPUTools.Physics.Scripts.Kernels
{
    public class CopySpecificParticlesKernel : KernelBase
    {
        [GpuData("particles")] public GpuBuffer<GPParticle> Particles { set; get; }
        [GpuData("outParticles")] public GpuBuffer<GPParticle> OutParticles { set; get; }
        [GpuData("outParticlesMap")] public GpuBuffer<float> OutParticlesMap { set; get; }

        public CopySpecificParticlesKernel() : base("Compute/CopySpecificParticles", "CSCopySpecificParticles")
        {

        }

        public override int GetGroupsNumX()
        {
            return 1;
        }
    }
}
