using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Behaviours;
using GPUTools.Physics.Scripts.Types.Shapes;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
    public class BuildStaticLineSpheres : BuildChainCommand
    {
        private readonly HairSettings settings;
        private readonly CacheProvider<LineSphereCollider> collidersCache;

        public BuildStaticLineSpheres(HairSettings settings)
        {
            this.settings = settings;
            collidersCache = new CacheProvider<LineSphereCollider>(settings.PhysicsSettings.ColliderProviders);
        }

        protected override void OnBuild()
        {
            if (collidersCache.Items.Count == 0)
                return;

            var staticLineSpheres = new GPLineSphere[collidersCache.Items.Count];
            ComputeStaticSpheres(staticLineSpheres);

            var oldStaticSpheres = new GPLineSphere[collidersCache.Items.Count];
            ComputeStaticSpheres(oldStaticSpheres);

            settings.RuntimeData.StaticLineSpheres = new GpuBuffer<GPLineSphere>(staticLineSpheres, GPLineSphere.Size());
            settings.RuntimeData.OldStaticLineSpheres = new GpuBuffer<GPLineSphere>(oldStaticSpheres, GPLineSphere.Size());
        }

        protected override void OnDispatch()
        {
            if (settings.RuntimeData.StaticLineSpheres == null)
                return;

            CopyStaticSpheres(settings.RuntimeData.StaticLineSpheres.Data, settings.RuntimeData.OldStaticLineSpheres.Data);
            settings.RuntimeData.OldStaticLineSpheres.PushData();

            ComputeStaticSpheres(settings.RuntimeData.StaticLineSpheres.Data);
            settings.RuntimeData.StaticLineSpheres.PushData();
        }

        private void ComputeStaticSpheres(GPLineSphere[] lineSpheres)
        {
            var colliders = collidersCache.Items;

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

        private void CopyStaticSpheres(GPLineSphere[] from, GPLineSphere[] to)
        {
            for (int i = 0; i < from.Length; i++)
            {
                to[i] = from[i];
            }
        }

        protected override void OnDispose()
        {
            if (settings.RuntimeData.StaticLineSpheres == null)
                return;

            settings.RuntimeData.StaticLineSpheres.Dispose();
            settings.RuntimeData.OldStaticLineSpheres.Dispose();
        }
    }
}
