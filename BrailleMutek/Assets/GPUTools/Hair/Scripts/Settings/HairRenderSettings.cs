using System;
using GPUTools.Hair.Scripts.Settings.Abstract;
using GPUTools.Hair.Scripts.Settings.Colors;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Hair.Scripts.Settings
{
    public enum ColorProviderType { RootTip, List, Geometry }

    [Serializable]
    public class HairRenderSettings : HairSettingsBase
    {
        //color
        public ColorProviderType ColorProviderType = ColorProviderType.RootTip;
        public RootTipColorProvider RootTipColorProvider;
        public ListColorProvider ListColorProvider;
        public GeometryColorProvider GeometryColorProvider;

        //specular
        public float PrimarySpecular = 50;
        public float SecondarySpecular = 50;
        public UnityEngine.Color SpecularColor = new UnityEngine.Color(0.15f, 0.15f, 0.15f);

        //diffuse
        public float Diffuse = 0.75f;
        public float FresnelPower = 10f;
        public float FresnelAttenuation = 1f;

        //lenght
        public float Length1 = 1;
        public float Length2 = 1;
        public float Length3 = 1;

        //waviness
        public float WavinessScale = 0;
        public AnimationCurve WavinessScaleCurve = AnimationCurve.EaseInOut(0,0,1,1);
        public float WavinessFrequency = 0;
        public AnimationCurve WavinessFrequencyCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public Vector3 WavinessAxis = new Vector3(0,1,0);

        //interpoation
        public AnimationCurve InterpolationCurve = AnimationCurve.EaseInOut(0, 0, 1, 0);
        
        //width
        public AnimationCurve WidthCurve = AnimationCurve.Linear(0, 1, 1, 1);

        //volume 
        public float Volume = 0;
        public float BarycentricVolume = 0.015f;
        
        public IColorProvider ColorProvider
        {
            get
            {
                if (ColorProviderType == ColorProviderType.RootTip)
                    return RootTipColorProvider;
                if (ColorProviderType == ColorProviderType.List)
                    return ListColorProvider;

                return GeometryColorProvider;
            }
        }

    }
}
