using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Physics.Scripts.Behaviours;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create.Kernels
{
    public class HairEditorKernel : KernelBase
    {
        private readonly HairGeometryCreator creator;
        private readonly CacheProvider<SphereCollider> sphereCollidersCache;
        private readonly CacheProvider<LineSphereCollider> lineSphereCollidersCache;

        [GpuData("vertices")] public GpuBuffer<Vector3> Vertices { get; set; }
        [GpuData("colors")] public GpuBuffer<Color> Colors { get; set; }
        [GpuData("distances")] public GpuBuffer<float> Distances { get; set; }
        [GpuData("matrices")] public GpuBuffer<Matrix4x4> Matrices { get; set; }
        [GpuData("staticSpheres")] public GpuBuffer<GPSphere> StaticSpheres { get; set; }
        [GpuData("staticLineSpheres")] public GpuBuffer<GPLineSphere> StaticLineSpheres { get; set; }

        [GpuData("segments")] public GpuValue<int> Segments { get; set; }

        [GpuData("brushPosition")] public GpuValue<Vector3> BrushPosition { get; set; }
        [GpuData("brushRadius")] public GpuValue<float> BrushRadius { get; set; }
        [GpuData("brushLenght1")] public GpuValue<float> BrushLenght1 { get; set; }
        [GpuData("brushLenght2")] public GpuValue<float> BrushLenght2 { get; set; }
        [GpuData("brushStrength")] public GpuValue<float> BrushStrength { get; set; }
        [GpuData("brushCollisionDistance")] public GpuValue<float> BrushCollisionDistance { get; set; }
        [GpuData("brushSpeed")] public GpuValue<Vector3> BrushSpeed { get; set; }
        [GpuData("brushLengthSpeed")] public GpuValue<float> BrushLengthSpeed { get; set; }
        [GpuData("brushColor")] public GpuValue<Vector3> BrushColor { get; set; }
        
        
        public HairEditorKernel(Vector3[] vertices, Color[] colors, float[] distances, HairGeometryCreator creator, string kernelName) : base("Compute/HairEditor", kernelName)
        {
            this.creator = creator;
            sphereCollidersCache = new CacheProvider<SphereCollider>(creator.ColliderProviders);
            lineSphereCollidersCache = new CacheProvider<LineSphereCollider>(creator.ColliderProviders);

            Vertices = new GpuBuffer<Vector3>(vertices, sizeof(float)*3);
            Distances = new GpuBuffer<float>(distances, sizeof(float));
            Colors = new GpuBuffer<Color>(colors, sizeof(float)*4);

            Matrices = new GpuBuffer<Matrix4x4>(new Matrix4x4[3], sizeof(float)*16);
            if(sphereCollidersCache.Items.Count > 0)
                StaticSpheres = new GpuBuffer<GPSphere>(sphereCollidersCache.Items.Count, GPSphere.Size());

            if(lineSphereCollidersCache.Items.Count > 0)
                StaticLineSpheres = new GpuBuffer<GPLineSphere>(lineSphereCollidersCache.Items.Count, GPLineSphere.Size());

            Segments = new GpuValue<int>();

            BrushPosition = new GpuValue<Vector3>();
            BrushRadius = new GpuValue<float>();
            BrushLenght1 = new GpuValue<float>();
            BrushLenght2 = new GpuValue<float>();
            BrushStrength = new GpuValue<float>();
            BrushCollisionDistance = new GpuValue<float>();
            BrushSpeed = new GpuValue<Vector3>();
            BrushLengthSpeed = new GpuValue<float>();
            BrushColor = new GpuValue<Vector3>();
        }

        private void ComputeStaticSpheres(GPSphere[] spheres)
        {
            var colliders = sphereCollidersCache.Items;

            if (spheres == null)
                spheres = new GPSphere[colliders.Count];

            for (var i = 0; i < colliders.Count; i++)
            {
                var sphereCollider = colliders[i];
                var transformedPoint = sphereCollider.transform.TransformPoint(sphereCollider.center);
                var transformedRadius = sphereCollider.transform.lossyScale.x * sphereCollider.radius;

                spheres[i] = new GPSphere(transformedPoint, transformedRadius);
            }
        }

        private void ComputeStaticSpheres(GPLineSphere[] lineSpheres)
        {
            var colliders = lineSphereCollidersCache.Items;

            if (lineSpheres == null)
                lineSpheres = new GPLineSphere[colliders.Count];

            for (var i = 0; i < colliders.Count; i++)
            {
                var lineSphereCollider = colliders[i];

                var worldRadiusA = lineSphereCollider.WorldRadiusA;
                var worldRadiusB = lineSphereCollider.WorldRadiusB;

                var worldA = lineSphereCollider.WorldA;
                var worldB = lineSphereCollider.WorldB;

                lineSpheres[i] = new GPLineSphere(worldA, worldB, worldRadiusA, worldRadiusB);
            }
        }

        public override void Dispatch()
        {
            Matrices.Data[0] = Camera.current.transform.worldToLocalMatrix;
            Matrices.Data[1] = creator.ScalpProvider.ToWorldMatrix;
            Matrices.Data[2] = creator.ScalpProvider.ToWorldMatrix.inverse;
            Matrices.PushData();

            if (StaticSpheres != null)
            {
                ComputeStaticSpheres(StaticSpheres.Data);
                StaticSpheres.PushData();
            }

            if (StaticLineSpheres != null)
            {
                ComputeStaticSpheres(StaticLineSpheres.Data);
                StaticLineSpheres.PushData();
            }

            Segments.Value = creator.Segments;

            BrushPosition.Value = creator.Brush.Position;
            BrushRadius.Value = creator.Brush.Radius;
            BrushLenght1.Value = creator.Brush.Lenght1;
            BrushLenght2.Value = creator.Brush.Lenght2;
            BrushStrength.Value = creator.Brush.Strength;
            BrushCollisionDistance.Value = creator.Brush.CollisionDistance;
            BrushSpeed.Value = creator.Brush.Speed;
            BrushColor.Value = new Vector3(creator.Brush.Color.r, creator.Brush.Color.g, creator.Brush.Color.b);

            base.Dispatch();
        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(Vertices.Count/ (float)GpuConfig.NumThreads);
        }

        public override void Dispose()
        {
            Vertices.Dispose();
            Distances.Dispose();
            Matrices.Dispose();
            Colors.Dispose();

            if(StaticSpheres != null)
                StaticSpheres.Dispose();

            if(StaticLineSpheres != null)
                StaticLineSpheres.Dispose();
        }
    }
}
