using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Colors
{
    [Serializable]
    public class RootTipColorProvider: IColorProvider
    {
        public UnityEngine.Color RootColor = new UnityEngine.Color(0.35f, 0.15f, 0.15f);
        public UnityEngine.Color TipColor = new UnityEngine.Color(0.15f, 0.05f, 0.05f);
        public AnimationCurve Blend = AnimationCurve.EaseInOut(0,0,1,1);

        public UnityEngine.Color GetColor(HairSettings settings, int x, int y, int sizeY)
        {
            return GetStandColor((float)y / sizeY);
        }

        private UnityEngine.Color GetStandColor(float t)
        {
            var blend = Blend.Evaluate(t);
            return UnityEngine.Color.Lerp(RootColor, TipColor, blend);
        }
    }
}
