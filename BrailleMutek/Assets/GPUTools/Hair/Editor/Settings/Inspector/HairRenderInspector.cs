using Assets.GPUTools.Common.Editor;
using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts;
using GPUTools.Hair.Scripts.Settings;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Settings.Inspector
{
    public class HairRenderInspector : EditorItemBase
    {
        private HairSettings settings;

        public HairRenderInspector(HairSettings settings)
        {
            this.settings = settings;
        }

        public override void DrawInspector()
        {
            Render.IsVisible = EditorGUILayout.Foldout(Render.IsVisible, "Render");
            if (!Render.IsVisible)
                return;
            
            EditorGUI.BeginChangeCheck();
            
            HeadCenter();
            //color
            //GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Color", EditorStyles.boldLabel);
            Render.ColorProviderType =
                (ColorProviderType) EditorGUILayout.EnumPopup("Type", Render.ColorProviderType);

            if (Render.ColorProviderType == ColorProviderType.RootTip)
                DrawRootTipColorProvider();
            else if (Render.ColorProviderType == ColorProviderType.List)
                DrawListColorProvider();
            else
                DrawGeometryrColorProvider();

            GUILayout.Label("Lighting", EditorStyles.boldLabel);
            //specular
            Render.PrimarySpecular = EditorGUILayout.FloatField("Primary Specular", Render.PrimarySpecular);
            Render.SecondarySpecular = EditorGUILayout.FloatField("Secondary Specular", Render.SecondarySpecular);
            Render.SpecularColor = EditorGUILayout.ColorField("Specular Color", Render.SpecularColor);

            //Freshnel
            Render.FresnelPower = Mathf.Clamp(EditorGUILayout.FloatField("Fresnel Power", Render.FresnelPower),0, 100);
            Render.FresnelAttenuation = Mathf.Clamp(EditorGUILayout.FloatField("Fresnel Attenuation", Render.FresnelAttenuation),0, 10);

            
            //waviness
            //Render.BarycentricVolume = EditorGUILayout.FloatField("Volume", Render.BarycentricVolume);
            //Render.WavinessAxis = EditorGUILayout.Vector3Field("Waviness Axis", Render.WavinessAxis); todo set per stand axis and start 
            GUILayout.Label("Curliness", EditorStyles.boldLabel);
            Render.WavinessAxis = EditorGUILayout.Vector3Field("Axis", Render.WavinessAxis);
            CurveScale("Root-Tip Scale", ref Render.WavinessScaleCurve, ref Render.WavinessScale);
            CurveScale("Root-Tip Frequency", ref Render.WavinessFrequencyCurve, ref Render.WavinessFrequency);




            GUILayout.Label("Volume", EditorStyles.boldLabel);
            Render.Length1 = EditorGUILayout.Slider(Render.Length1, 0, 1);
            Render.Length2 = EditorGUILayout.Slider(Render.Length2, 0, 1);
            Render.Length3 = EditorGUILayout.Slider(Render.Length3, 0, 1);

            Render.InterpolationCurve = EditorGUILayout.CurveField("Root-Tip Interpolation", Render.InterpolationCurve);

            if (EditorGUI.EndChangeCheck())
            {
                settings.UpdateSettings();
                EditorUtility.SetDirty(settings.gameObject);
            }
        }
        
        private void HeadCenter()
        {
            GUILayout.Label("Lighting Center", EditorStyles.boldLabel);
            Geometry.HeadCenterType = (HairHeadCenterType) EditorGUILayout.EnumPopup("Lighting Center Type", Geometry.HeadCenterType);
            if(Geometry.HeadCenterType == HairHeadCenterType.LocalPoint)
                Geometry.HeadCenter = EditorGUILayout.Vector3Field("Center", Geometry.HeadCenter);
            if(Geometry.HeadCenterType == HairHeadCenterType.Transform)
                Geometry.HeadCenterTransform = (Transform) EditorGUILayout.ObjectField("Center", Geometry.HeadCenterTransform, typeof(Transform), true);
        }

        public void CurveScale(string name, ref AnimationCurve curve, ref float scale)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label(name);
            EditorGUILayout.BeginHorizontal();
            scale = EditorGUILayout.FloatField(scale);
            curve = EditorGUILayout.CurveField("", curve);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        public void DrawRootTipColorProvider()
        {
            var provider = Render.RootTipColorProvider;
            provider.RootColor = EditorGUILayout.ColorField("Root Color", provider.RootColor);
            provider.TipColor = EditorGUILayout.ColorField("Tip Color", provider.TipColor);
            provider.Blend = EditorGUILayout.CurveField("Color Blend", provider.Blend);
        }

        public void DrawListColorProvider()
        {
            EditorDrawUtils.ListColorGUI("Color", Render.ListColorProvider.Colors);
        }

        public void DrawGeometryrColorProvider()
        {

        }
      
        public HairStandsSettings Geometry
        {
            get { return settings.StandsSettings; }
        }

        public HairRenderSettings Render
        {
            get { return settings.RenderSettings; }
        }

        public override void DrawScene()
        {
            Handles.SphereHandleCap(0,Geometry.HeadCenterWorld, Quaternion.identity, 0.025f, EventType.Repaint);
        }
    }
}
