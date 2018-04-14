using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Wind;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
    public class BuildWind : BuildChainCommand
    {
        private readonly HairSettings settings;
        private readonly WindReceiver wind;

        public BuildWind(HairSettings settings)
        {
            this.settings = settings;
            wind = new WindReceiver();
        }

        protected override void OnDispatch()
        {
            settings.RuntimeData.Wind = wind.GetWind(settings.StandsSettings.HeadCenterWorld) * settings.PhysicsSettings.WindMultiplier;
        }
    }
}
