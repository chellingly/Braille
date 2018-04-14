using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Colors
{
    [Serializable]
    public class ListColorProvider : IColorProvider
    {
        public List<UnityEngine.Color> Colors = new List<UnityEngine.Color>();

        public UnityEngine.Color GetColor(HairSettings settings, int x, int y, int sizeY)
        {
            return GetStandColor((float) y/sizeY);
        }

        private UnityEngine.Color GetStandColor(float t)
        {
            if (Colors.Count == 0)
                return UnityEngine.Color.black;

            var i = Colors.Count*t;
            var iClamped = (int) Mathf.Clamp(i, 0, Colors.Count - 1);
            
            return Colors[iClamped];
        }
    }
}
