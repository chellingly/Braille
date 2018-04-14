using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Constrains;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Data
{
    public class HairDataFacade
    {
        private readonly HairSettings settings;

        public HairDataFacade(HairSettings settings)
        {
            this.settings = settings;
        }

        #region Config

        public bool DebugDraw { get { return settings.PhysicsSettings.DebugDraw; }}
        public int Iterations  { get { return settings.PhysicsSettings.Iterations; }}
        public bool FastMovement  { get { return settings.PhysicsSettings.FastMovement; }}
        public Vector3 Gravity { get { return settings.PhysicsSettings.Gravity; }}
        public float InvDrag { get { return 1 - Mathf.Clamp01(settings.PhysicsSettings.Drag); }}
        public Vector3 Wind { get { return settings.RuntimeData.Wind; }}
        public bool IsPhysicsEnabled { get { return settings.PhysicsSettings.IsEnabled; }}
        public bool UseDeltaTime { get { return settings.PhysicsSettings.UseDeltaTime; }}
        public float DeltaTime { get { return settings.DeltaTime.GetSmoothedValue(); }}

        #endregion

        #region Transforms

        public GpuBuffer<Matrix4x4> MatricesBuffer { get { return settings.StandsSettings.Provider.GetTransformsBuffer(); } }

        #endregion

        #region Stands

        public GpuBuffer<GPParticle> Particles { get { return settings.RuntimeData.Particles; }}
        public GroupedData<GPDistanceJoint> DistanceJoints { get { return settings.RuntimeData.DistanceJoints; }}
        public GpuBuffer<GPPointJoint> PointJoints { get { return settings.RuntimeData.PointJoints; }}

        public List<HairJointArea> JointAreas{ get { return settings.PhysicsSettings.JointAreas; }}

        public Vector4 Size
        {
            get
            {
                var sizeX = settings.StandsSettings.Provider.GetStandsNum();
                var sizeY = settings.StandsSettings.Provider.GetSegmentsNum();

                return new Vector4(sizeX, sizeY);
            }
        }

        #endregion

        #region Kinematic

        public GpuBuffer<GPSphere> StaticSpheres { get { return settings.RuntimeData.StaticSpheres; }}
        public GpuBuffer<GPLineSphere> StaticLineSpheres { get { return settings.RuntimeData.StaticLineSpheres; }}
        public GpuBuffer<GPSphere> OldStaticSpheres { get { return settings.RuntimeData.OldStaticSpheres; } }
        public GpuBuffer<GPLineSphere> OldStaticLineSpheres { get { return settings.RuntimeData.OldStaticLineSpheres; } }
        public GpuBuffer<TessRenderParticle> TessRenderParticles { get { return settings.RuntimeData.TessRenderParticles; }}
        public GpuBuffer<GPParticle> OutParticles { get { return settings.RuntimeData.OutParticles; }}
        public GpuBuffer<float> OutParticlesMap { get { return settings.RuntimeData.OutParticlesMap; }}
        public GpuBuffer<RenderParticle> RenderParticles { get { return settings.RuntimeData.RenderParticles; }}
        
        #endregion
        
        #region Waviness

        public Vector3 WavinessAxis { get { return settings.RenderSettings.WavinessAxis; } }
        public Vector3 WorldWavinessAxis { get { return settings.transform.TransformVector(WavinessAxis); } }

        #endregion

        #region LOD

        public Vector3 LightCenter { get { return settings.StandsSettings.HeadCenterWorld; } }
        public float StandWidth { get { return settings.LODSettings.GetWidth(LightCenter); } }

        public Vector3 TessFactor
        {
            get
            {
                var x = settings.LODSettings.GetDetail(LightCenter);
                var y = settings.LODSettings.GetDencity(LightCenter);
                return new Vector4(x, y, 0.99f / x, 0.99f / y);
            }
        }

        public bool IsPhysicsEnabledLOD
        {
            get { return settings.LODSettings.IsPhysicsEnabled(LightCenter); }
        }

        #endregion
        
        #region Shadows

        public bool CastShadows { get { return settings.ShadowSettings.CastShadows; } }
        public bool ReseiveShadows { get { return settings.ShadowSettings.ReseiveShadows; } }

        #endregion
        
        #region Specular

        public float SpecularShift { get { return 0.01f; } }
        public float PrimarySpecular { get { return settings.RenderSettings.PrimarySpecular; } }
        public float SecondarySpecular { get { return settings.RenderSettings.SecondarySpecular; } }
        public Color SpecularColor { get { return settings.RenderSettings.SpecularColor; } }

        public float Diffuse { get { return settings.RenderSettings.Diffuse; } }
        public float FresnelPower { get { return settings.RenderSettings.FresnelPower; } }
        public float FresnelAttenuation { get { return settings.RenderSettings.FresnelAttenuation; } }

        #endregion
        
        #region Interpolation

        public Vector3 Length { get { return new Vector4(settings.RenderSettings.Length1, settings.RenderSettings.Length2, settings.RenderSettings.Length3); } }

        #endregion
        
        #region Render

        public GpuBuffer<RenderParticle> ParticlesData { get { return settings.RuntimeData.RenderParticles; } }
        public GpuBuffer<Vector3> Barycentrics { get { return settings.RuntimeData.Barycentrics; } }
        public int[] Indices { get { return settings.StandsSettings.Provider.GetIndices(); } }
        //public Bounds Bounds { get { return new Bounds(settings.StandsSettings.HeadCenterWorld, settings.StandsSettings.BoundsSize); } }
        public Bounds Bounds { get { return settings.StandsSettings.Provider.GetBounds(); } }
        public float Volume { get { return settings.RenderSettings.Volume; } }

        #endregion
    }
}
