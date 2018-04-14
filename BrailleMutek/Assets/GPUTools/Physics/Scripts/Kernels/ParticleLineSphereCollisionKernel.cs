using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
    public class ParticleLineSphereCollisionKernel : KernelBase
    {
        [GpuData("step")] public GpuValue<float> Step { set; get; }
        [GpuData("t")] public GpuValue<float> T { set; get; }
        [GpuData("particles")] public GpuBuffer<GPParticle> Particles { set; get; }
        [GpuData("staticLineSpheres")] public GpuBuffer<GPLineSphere> StaticLineSpheres { set; get; }
        [GpuData("oldStaticLineSpheres")] public GpuBuffer<GPLineSphere> OldStaticLineSpheres { set; get; }

        public ParticleLineSphereCollisionKernel() : base("Compute/ParticleLineSphereCollision", "CSParticleLineSphereCollision")
        {

        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(Particles.Count / (float)GpuConfig.NumThreads);
        }
    }
}
