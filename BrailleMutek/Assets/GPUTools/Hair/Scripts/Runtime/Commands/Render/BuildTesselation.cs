using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Runtime.Render;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Render
{
    public class BuildTesselation : IBuildCommand
    {
        private readonly HairSettings settings;

        public BuildTesselation(HairSettings settings)
        {
            this.settings = settings;
        }

        public void Build()
        {
            var maxVertices = settings.StandsSettings.Provider.GetStandsNum()*64;
            settings.RuntimeData.TessRenderParticles = new GpuBuffer<TessRenderParticle>(maxVertices, TessRenderParticle.Size());
        }

        public void Dispatch()
        {
            
        }

        public void UpdateSettings()
        {
             
        }

        public void Dispose()
        {
            settings.RuntimeData.TessRenderParticles.Dispose();
        }
    }
}
