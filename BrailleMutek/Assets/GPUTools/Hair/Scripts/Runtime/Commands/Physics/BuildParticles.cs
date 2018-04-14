using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Physics.Scripts.Types.Dynamic;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
    public class BuildParticles : BuildChainCommand
    {
        private readonly HairSettings settings;
        private readonly GeometryProviderBase provider;

        public BuildParticles(HairSettings settings)
        {
            this.settings = settings;
            provider = settings.StandsSettings.Provider;
        }

        protected override void OnBuild()
        {
            var particles = new GPParticle[provider.GetVertices().Count];
            ComputeParticles(particles);

            settings.RuntimeData.Particles = new GpuBuffer<GPParticle>(particles, GPParticle.Size());
        }

        protected override void OnUpdateSettings()
        {
            ComputeParticles(settings.RuntimeData.Particles.Data);
            settings.RuntimeData.Particles.PushData();
        }

        private void ComputeParticles(GPParticle[] particles)
        {
            var matrix = provider.GetToWorldMatrix();
            var radius = settings.PhysicsSettings.StandRadius * provider.transform.lossyScale.x;
            var vertices = provider.GetVertices();

            for (var i = 0; i < vertices.Count; i++)
            {
                var vertex = matrix.MultiplyPoint3x4(vertices[i]);
                particles[i] = new GPParticle(vertex, radius);
            }
        }

        protected override void OnDispose()
        {
            settings.RuntimeData.Particles.Dispose();
        }
    }
}
