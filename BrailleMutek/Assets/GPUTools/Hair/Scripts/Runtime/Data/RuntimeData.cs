using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Data
{
    public class RuntimeData
    {
        public GpuBuffer<GPParticle> Particles { get; set; }
        public GpuBuffer<GPSphere> StaticSpheres { get; set; }
        public GpuBuffer<GPLineSphere> StaticLineSpheres { get; set; }
        public GpuBuffer<GPSphere> OldStaticSpheres { get; set; }
        public GpuBuffer<GPLineSphere> OldStaticLineSpheres { get; set; }
        public GroupedData<GPDistanceJoint> DistanceJoints { get; set; }
        public GpuBuffer<GPPointJoint> PointJoints { get; set; }

        public GpuBuffer<Vector3> Barycentrics { get; set; }
        public GpuBuffer<RenderParticle> RenderParticles { get; set; }
        public GpuBuffer<TessRenderParticle> TessRenderParticles { get; set; }

        public GpuBuffer<GPParticle> OutParticles { get; set; }
        public GpuBuffer<float> OutParticlesMap { get; set; }

        public Vector3 Wind { get; set; }
    }
}
