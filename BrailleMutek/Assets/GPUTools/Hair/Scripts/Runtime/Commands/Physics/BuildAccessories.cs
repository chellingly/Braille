using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
    public class BuildAccessories : BuildChainCommand
    {
        private readonly HairSettings settings;
        private CacheProvider<SphereCollider> sphereCollidersCache;

        public BuildAccessories(HairSettings settings)
        {
            this.settings = settings;
            sphereCollidersCache = new CacheProvider<SphereCollider>(settings.PhysicsSettings.AccessoriesProviders);
        }

        protected override void OnBuild()
        {
            if(sphereCollidersCache.Items.Count == 0)
                return;

            var outParticles = new GPParticle[sphereCollidersCache.Items.Count];
            settings.RuntimeData.OutParticles = new GpuBuffer<GPParticle>(outParticles, GPParticle.Size());

            var outParticlesMap = new float[sphereCollidersCache.Items.Count];
            CalculateOutParticlesMap(outParticlesMap);
            settings.RuntimeData.OutParticlesMap = new GpuBuffer<float>(outParticlesMap, sizeof(float));
        }

        protected override void OnDispatch()
        {
            if(settings.RuntimeData.OutParticles == null)
                return;

            settings.RuntimeData.OutParticles.PullData();
            for (var i = 0; i < settings.RuntimeData.OutParticles.Data.Length; i++)
            {
                var particle = settings.RuntimeData.OutParticles.Data[i];
                sphereCollidersCache.Items[i].transform.position = particle.Position;
            }
        }

        private void CalculateOutParticlesMap(float[] outParticlesMap)
        {
            var particleToColliderMinDistance = new float[sphereCollidersCache.Items.Count];
            for (var i = 0; i < particleToColliderMinDistance.Length; i++)
                particleToColliderMinDistance[i] = float.PositiveInfinity;

            var particles = settings.RuntimeData.Particles.Data;

            for (var i = 0; i < particles.Length; i++)
            {
                var particle = particles[i];

                for (var j = 0; j < sphereCollidersCache.Items.Count; j++)
                {
                    var collider = sphereCollidersCache.Items[j];
                    var distance = Vector3.Distance(collider.transform.position, particle.Position);

                    if (distance < collider.radius && distance < particleToColliderMinDistance[j])
                    {
                        particleToColliderMinDistance[j] = distance;
                        outParticlesMap[j] = i;
                    }
                }
            }
        }

        protected override void OnDispose()
        {
            if(settings.RuntimeData.OutParticles != null)
                settings.RuntimeData.OutParticles.Dispose();

            if (settings.RuntimeData.OutParticlesMap != null)
                settings.RuntimeData.OutParticlesMap.Dispose();
        }
    }
}
