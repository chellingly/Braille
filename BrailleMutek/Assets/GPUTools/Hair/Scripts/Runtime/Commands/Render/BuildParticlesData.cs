using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Runtime.Render;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Render
{
    public class BuildParticlesData : IBuildCommand
    {
        private readonly HairSettings settings;

        public BuildParticlesData(HairSettings settings)
        {
            this.settings = settings;
        }

        public void Build()
        {
            var particlesData = new RenderParticle[settings.RuntimeData.Particles.Count];
            UpdateBodies(particlesData);

            settings.RuntimeData.RenderParticles = new GpuBuffer<RenderParticle>(particlesData, RenderParticle.Size());
        }
        
        public void UpdateSettings()
        {
            UpdateBodies(settings.RuntimeData.RenderParticles.Data);
            settings.RuntimeData.RenderParticles.PushData();
        }

        private void UpdateBodies(RenderParticle[] renderParticles)
        {
            var renderSettings = settings.RenderSettings;
            var sizeY = settings.StandsSettings.Provider.GetSegmentsNum();

            for (var i = 0; i < renderParticles.Length; i++)
            {
                var x = i / sizeY;
                var y = i % sizeY;
                var t = (float)y / sizeY;

                var data = new RenderParticle
                {
                    Color = ColorToVector(renderSettings.ColorProvider.GetColor(settings, x, y, sizeY)),
                    Interpolation = Mathf.Clamp01(renderSettings.InterpolationCurve.Evaluate(t)),
                    WavinessScale = Mathf.Clamp01(renderSettings.WavinessScaleCurve.Evaluate(t)) * renderSettings.WavinessScale,
                    WavinessFrequency = Mathf.Clamp01(renderSettings.WavinessFrequencyCurve.Evaluate(t)) * renderSettings.WavinessFrequency,
                };

                renderParticles[i] = data;
            }
        }

        public Vector3 ColorToVector(Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public void Dispatch()
        {

        }

        public void Dispose()
        {
            settings.RuntimeData.RenderParticles.Dispose();
        }
    }
}
