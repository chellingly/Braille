using System.Collections.Generic;
using Assets.GPUTools.Common.Editor;
using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts;
using GPUTools.Hair.Scripts.Geometry.Constrains;
using GPUTools.Hair.Scripts.Settings;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Settings.Inspector
{
    public class HairPhysicsInspector : EditorItemBase
    {
        private HairSettings settings;

        public HairPhysicsInspector(HairSettings settings)
        {
            this.settings = settings;
        }

        public override void DrawInspector()
        {
            EditorGUI.BeginChangeCheck();
            
            EditorDrawUtils.Folder("Physics", ref Physics.IsVisible, ref Physics.IsEnabled);
            if (!Physics.IsVisible || !Physics.IsEnabled)
                return;

            GUILayout.Label("Solver", EditorStyles.boldLabel);
            //Physics.Iterations = EditorGUILayout.IntSlider("Iterations", Physics.Iterations, 1, 30);
            //Physics.FastMovement = EditorGUILayout.Toggle("Fast Movement", Physics.FastMovement);
            //Physics.UseDeltaTime = EditorGUILayout.Toggle("Use Delta Time", Physics.UseDeltaTime);
            Physics.Gravity = EditorGUILayout.Vector3Field("Gravity", Physics.Gravity);
            
            GUILayout.Label("Stands", EditorStyles.boldLabel);
            Physics.StandRadius = Mathf.Clamp(EditorGUILayout.FloatField("Radius", Physics.StandRadius), 0, 1);
            Physics.Drag = EditorGUILayout.Slider("Drag", Physics.Drag, 0, 1);
            Physics.ElasticityCurve = EditorGUILayout.CurveField("Root-Tip Elasticity", Physics.ElasticityCurve);
            
            GUILayout.Label("Wind", EditorStyles.boldLabel);
            Physics.WindMultiplier =
                Mathf.Max(EditorGUILayout.FloatField("Multiplier", Physics.WindMultiplier), 0);

            EditorDrawUtils.ListObjectGUI("Collider", Physics.ColliderProviders);
            EditorDrawUtils.ListObjectGUI("Accessorie", Physics.AccessoriesProviders);
            EditorDrawUtils.ListObjectGUI("Joint", Physics.JointAreas);

            if (EditorGUI.EndChangeCheck())
            {
                settings.UpdateSettings();
            }
        }

        public HairPhysicsSettings Physics
        {
            get { return settings.PhysicsSettings; }
        }
    }
}
