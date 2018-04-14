using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
    public class ParticleSphereCollisionKernel : KernelBase
    {
        [GpuData("step")] public GpuValue<float> Step { set; get; }
        [GpuData("t")] public GpuValue<float> T { set; get; }
        [GpuData("particles")] public GpuBuffer<GPParticle> Particles { set; get; }
        [GpuData("staticSpheres")] public GpuBuffer<GPSphere> StaticSpheres { set; get; }
        [GpuData("oldStaticSpheres")] public GpuBuffer<GPSphere> OldStaticSpheres { set; get; }

        public ParticleSphereCollisionKernel() : base("Compute/ParticleSphereCollision", "CSParticleSphereCollision")
        {

        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(Particles.Count/ (float)GpuConfig.NumThreads);
        }
    }
}
