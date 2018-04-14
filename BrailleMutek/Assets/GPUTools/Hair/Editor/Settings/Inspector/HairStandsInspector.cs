using System;
using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Settings;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Settings.Inspector
{
    public class HairStandsInspector : EditorItemBase
    {
        private HairSettings settings;

        public HairStandsInspector(HairSettings settings)
        {
            this.settings = settings;
        }

        public override void DrawInspector()
        {        
            Geometry.IsVisible = EditorGUILayout.Foldout(Geometry.IsVisible, "Geometry");
            if (!Geometry.IsVisible)
                return;

            Geometry.Provider = (GeometryProviderBase)EditorGUILayout.ObjectField("Provider", Geometry.Provider, typeof(GeometryProviderBase), true);

            EditorGUI.BeginChangeCheck();

            //Geometry.BoundsSize = EditorGUILayout.Vector3Field("Size", Geometry.BoundsSize);

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }          
        }

        public HairStandsSettings Geometry
        {
            get { return settings.StandsSettings; }
        }

        public override bool Validate()
        {
            return Geometry.Provider != null;
        }
    }
}
