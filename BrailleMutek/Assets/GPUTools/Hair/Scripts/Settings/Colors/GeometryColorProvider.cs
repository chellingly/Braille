using System;

namespace GPUTools.Hair.Scripts.Settings.Colors
{
    [Serializable]
    public class GeometryColorProvider : IColorProvider
    {
        public UnityEngine.Color GetColor(HairSettings settings, int x, int y, int sizeY)
        {
            var colors = settings.StandsSettings.Provider.GetColors();
            var i = x*sizeY + y;

            return colors[i];
        }
    }
}
