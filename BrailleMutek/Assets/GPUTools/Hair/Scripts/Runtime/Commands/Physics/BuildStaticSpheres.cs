using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
    public class BuildStaticSpheres : BuildChainCommand
    {
        private readonly HairSettings settings;
        private readonly CacheProvider<SphereCollider> sphereCollidersCache;

        public BuildStaticSpheres(HairSettings settings)
        {
            this.settings = settings;
            sphereCollidersCache = new CacheProvider<SphereCollider>(settings.PhysicsSettings.ColliderProviders);
        }

        protected override void OnBuild()
        {
            if(sphereCollidersCache.Items.Count == 0)
                return;


            var staticSpheres = new GPSphere[sphereCollidersCache.Items.Count];
            ComputeStaticSpheres(staticSpheres);

            var oldStaticSpheres = new GPSphere[sphereCollidersCache.Items.Count];
            ComputeStaticSpheres(oldStaticSpheres);

            settings.RuntimeData.StaticSpheres = new GpuBuffer<GPSphere>(staticSpheres, GPSphere.Size());
            settings.RuntimeData.OldStaticSpheres = new GpuBuffer<GPSphere>(oldStaticSpheres, GPSphere.Size());
        }

        protected override void OnDispatch()
        {
            if(settings.RuntimeData.StaticSpheres == null)
                return;

            CopyStaticSpheres(settings.RuntimeData.StaticSpheres.Data, settings.RuntimeData.OldStaticSpheres.Data);
            settings.RuntimeData.OldStaticSpheres.PushData();

            ComputeStaticSpheres(settings.RuntimeData.StaticSpheres.Data);
            settings.RuntimeData.StaticSpheres.PushData();
        }

        private void ComputeStaticSpheres(GPSphere[] staticSpheres)
        {
            var colliders = sphereCollidersCache.Items;

            for (var i = 0; i < colliders.Count; i++)
            {
                var sphereCollider = colliders[i];

                var transformedRadius = sphereCollider.transform.lossyScale.x * sphereCollider.radius;
                var transformedPoint = sphereCollider.transform.TransformPoint(sphereCollider.center);

                staticSpheres[i] = new GPSphere(transformedPoint, transformedRadius);
            }
        }

        private void CopyStaticSpheres(GPSphere[] from, GPSphere[] to)
        {
            for (int i = 0; i < from.Length; i++)
            {
                to[i] = from[i];
            }
        }


        protected override void OnDispose()
        {
            if (settings.RuntimeData.StaticSpheres == null)
                return;

            settings.RuntimeData.StaticSpheres.Dispose();
            settings.RuntimeData.OldStaticSpheres.Dispose();
        }
    }
}
