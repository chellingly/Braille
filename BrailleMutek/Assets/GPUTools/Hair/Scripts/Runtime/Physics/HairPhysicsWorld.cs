using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Kernels;
using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Runtime.Kernels;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.DebugDraw;
using GPUTools.Physics.Scripts.Kernels;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Physics
{
    public class HairPhysicsWorld : PrimitiveBase
    {
        private readonly HairDataFacade data;

        [GpuData("step")] public GpuValue<float> Step { set; get; }
        [GpuData("t")] public GpuValue<float> T { set; get; }
        [GpuData("dt")] public GpuValue<float> DT { set; get; }
        [GpuData("invDrag")] public GpuValue<float> InvDrag { set; get; }
        [GpuData("gravity")] public GpuValue<Vector3> Gravity { set; get; }
        [GpuData("wind")] public GpuValue<Vector3> Wind { set; get; }

        [GpuData("particles")] public GpuBuffer<GPParticle> Particles { set; get; }

        [GpuData("transforms")] public GpuBuffer<Matrix4x4> Transforms { set; get; }
        [GpuData("oldTransforms")] public GpuBuffer<Matrix4x4> OldTransforms { set; get; }
        [GpuData("pointJoints")] public GpuBuffer<GPPointJoint> PointJoints { set; get; }
        [GpuData("isFixed")] public GpuValue<int> IsFixed { set; get; }
        [GpuData("staticSpheres")] public GpuBuffer<GPSphere> StaticSpheres { set; get; }
        [GpuData("staticLineSpheres")] public GpuBuffer<GPLineSphere> StaticLineSpheres { set; get; }
        [GpuData("oldStaticSpheres")] public GpuBuffer<GPSphere> OldStaticSpheres { set; get; }
        [GpuData("oldStaticLineSpheres")] public GpuBuffer<GPLineSphere> OldStaticLineSpheres { set; get; }

        [GpuData("outParticles")] public GpuBuffer<GPParticle> OutParticles { set; get; }
        [GpuData("outParticlesMap")] public GpuBuffer<float> OutParticlesMap { set; get; }

        [GpuData("renderParticles")] public GpuBuffer<RenderParticle> RenderParticles { set; get; }
        [GpuData("tessRenderParticles")] public GpuBuffer<TessRenderParticle> TessRenderParticles { set; get; }
        [GpuData("segments")] public GpuValue<int> Segments { set; get; }
        [GpuData("tessSegments")] public GpuValue<int> TessSegments { set; get; }
        [GpuData("wavinessAxis")] public GpuValue<Vector3> WavinessAxis { set; get; }
        [GpuData("lightCenter")] public GpuValue<Vector3> LightCenter { set; get; }

        private GPUMatrixCopyPaster matrixCopyPaster;
        private ResetKernel resetKernel;
        private IntegrateKernel integrateKernel;
        private DistanceJointsKernel distanceJointsKernel;
        private int frame;

        private List<KernelBase> staticQueue = new List<KernelBase>();
        private bool isPhysics;

        public HairPhysicsWorld(HairDataFacade data)
        {
            this.data = data;

            T = new GpuValue<float>();
            Step = new GpuValue<float>();
            DT = new GpuValue<float>();
            Gravity = new GpuValue<Vector3>();
            InvDrag = new GpuValue<float>();
            Wind = new GpuValue<Vector3>();
            Segments = new GpuValue<int>();
            TessSegments = new GpuValue<int>();
            WavinessAxis = new GpuValue<Vector3>();
            IsFixed = new GpuValue<int>();
            LightCenter = new GpuValue<Vector3>();

            InitData();
            InitBuffers();
            InitPasses();

            Bind();
        }

        private void InitData()
        {
            /*Step.Value = data.UseDeltaTime
                ? data.DeltaTime * Application.targetFrameRate / data.Iterations
                : 1f / data.Iterations;*/

            Step.Value = 1;
            DT.Value = Time.fixedDeltaTime;

            Gravity.Value = data.Gravity;
            InvDrag.Value = data.InvDrag;
            Wind.Value = data.Wind;
            Segments.Value = (int)data.Size.y;
            TessSegments.Value = (int)data.TessFactor.y;
            WavinessAxis.Value = data.WorldWavinessAxis;
            IsFixed.Value = isPhysics ? 0 : 1;
            LightCenter.Value = data.LightCenter;
        }

        private void InitBuffers()
        {
            Particles = data.Particles;
            Transforms = data.MatricesBuffer;
            OldTransforms = new GpuBuffer<Matrix4x4>(Transforms.Count, sizeof(float) * 16);
            PointJoints = data.PointJoints;
            StaticSpheres = data.StaticSpheres;
            StaticLineSpheres = data.StaticLineSpheres;
            RenderParticles = data.RenderParticles;
            TessRenderParticles = data.TessRenderParticles;
            OutParticles = data.OutParticles;
            OutParticlesMap = data.OutParticlesMap;
            OldStaticSpheres = data.OldStaticSpheres;
            OldStaticLineSpheres = data.OldStaticLineSpheres;
        }

        private void AddStaticPass(KernelBase kernel)
        {
            AddPass(kernel);
            staticQueue.Add(kernel);
        }

        private void InitPasses()
        {
            AddPass(integrateKernel = new IntegrateKernel());

            AddPass(distanceJointsKernel = new DistanceJointsKernel(data.DistanceJoints));
            /*if(data.FastMovement)
                AddPass(new SplineJointsKernel());*/
            AddPass(new SplineJointsKernel());

            if (data.StaticSpheres != null)
                AddPass(new ParticleSphereCollisionKernel());

            if (data.StaticLineSpheres != null)
                AddPass(new ParticleLineSphereCollisionKernel());

            AddStaticPass(new PointJointsKernel());

            if(data.OutParticles != null)
                AddPass(new CopySpecificParticlesKernel());

            AddStaticPass(new TesselateKernel());

            AddStaticPass(resetKernel = new ResetKernel());
            AddStaticPass(matrixCopyPaster = new GPUMatrixCopyPaster(Transforms, OldTransforms));
        }

        public void FixedDispatch()
        {
            InitData();
            if(!isPhysics)
                return;

            T.Value = 1;
            integrateKernel.IsEnabled = false;
            integrateKernel.Dispatch();
            integrateKernel.IsEnabled = true;
        }

        public override void Dispatch()
        {
            UpdateIsPhysics();
            InitData();

            if(isPhysics)
                DispatchPhysicsImpl();
            else
                DispatchStaticImpl();
        }

        private void DispatchPhysicsImpl()
        {
            if (frame < 2 && integrateKernel != null)
            {
                DispatchMatrixCopyPaste();
                resetKernel.IsEnabled = true;
                //integrateKernel.IsEnabled = false;
            }
            else if (integrateKernel != null)
            {
                resetKernel.IsEnabled = false;
                //integrateKernel.IsEnabled = true;
            }

            /*for (var i = 1; i <= data.Iterations; i++)
            {
                T.Value = (float)i / data.Iterations;
                
                base.Dispatch();
            }*/
            T.Value = 1;
            base.Dispatch();

            DispatchMatrixCopyPaste();
            frame++;
        }

        private void DispatchStaticImpl()
        {
            T.Value = 1;
            for (var i = 0; i < staticQueue.Count; i++)
            {
                staticQueue[i].Dispatch();
            }

            DispatchMatrixCopyPaste();
        }

        private void DispatchMatrixCopyPaste()
        {
            matrixCopyPaster.IsEnabled = true;
            matrixCopyPaster.Dispatch();
            matrixCopyPaster.IsEnabled = false;
        }

        public override void Dispose()
        {
            base.Dispose();
            OldTransforms.Dispose();
        }

        public void DebugDraw()
        {
            if(data.DebugDraw && data.IsPhysicsEnabled && data.IsPhysicsEnabledLOD)
                GPDebugDraw.Draw(distanceJointsKernel.DistanceJoints, Particles, false, true);
        }

        public void UpdateIsPhysics()
        {
            var value = data.IsPhysicsEnabled && data.IsPhysicsEnabledLOD;
            if (!isPhysics && value)
            {
                resetKernel.IsEnabled = true;
                resetKernel.Dispatch();
            }

            isPhysics = value;
        }
    }
}
