using System.Collections.Generic;
using Assets.GPUTools.Common.Editor.Engine;
using Assets.GPUTools.Hair.Editor.Geometry.Create.Scene;
using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Inspector
{
    public class CreatorBrushInspector : EditorItemBase
    {
        private EditorInput input = new EditorInput();
        private HairGeometryCreator creator;
        private const int Width = 330;
        private const int Height = 180;
        
        private Dictionary<GeometryBrushBehaviour, GPBaseBrush> brushes = new Dictionary<GeometryBrushBehaviour, GPBaseBrush>();

        public CreatorBrushInspector(HairGeometryCreator creator)
        {
            this.creator = creator;
            
            brushes.Add(GeometryBrushBehaviour.Move, new GPMoveBrush(creator));
            brushes.Add(GeometryBrushBehaviour.Remove, new GPRemoveBrush(creator));
            brushes.Add(GeometryBrushBehaviour.Grow, new GPShrinkBrush(creator, 0.001f));
            brushes.Add(GeometryBrushBehaviour.Shrink, new GPShrinkBrush(creator, -0.001f));
            brushes.Add(GeometryBrushBehaviour.Color, new GPColorBrush(creator));
        }

        public override void Dispose()
        {
            foreach (var brush in brushes)
                brush.Value.Dispose();
        }

        public override void DrawInspector()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Brush Settings", EditorStyles.boldLabel);
            creator.Brush.Color = EditorGUILayout.ColorField("Color", creator.Brush.Color);
    }

        public override void DrawScene()
        {
            if (!IsGeometryReady())
                return;

            if(creator.Brush.Behaviour != GeometryBrushBehaviour.None)
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            
            DrawWindow();
            UpdateBrush();
        }

        private void DrawWindow()
        {
            Handles.BeginGUI();

            GUILayout.BeginArea(GetWindowRect(), EditorStyles.helpBox);
            var rect = EditorGUILayout.BeginVertical();

            DrawTitle();
            DrawSeparator();
            DrawBrushBehaviour();
            DrawSeparator();
            DrawSettings();

            EditorGUILayout.EndVertical();
            
            GUI.backgroundColor = Color.clear;
            if (GUI.Button(rect, "", EditorStyles.helpBox)){ }

            GUILayout.EndArea();
            Handles.EndGUI();
        }
        
        private void DrawSettings()
        {
            var brush = creator.Brush;
            brush.Radius = EditorGUILayout.Slider("Radius", brush.Radius, 0, 1);
            brush.Strength = EditorGUILayout.Slider("Strength", brush.Strength, 0, 1);
            brush.CollisionDistance = EditorGUILayout.Slider("Collision Distance", brush.CollisionDistance, 0, 1);
            brush.Lenght1 = EditorGUILayout.Slider("Lenght Front", brush.Lenght1, 0, 1);
            brush.Lenght2 = EditorGUILayout.Slider("Lenght Back", brush.Lenght2, 0, 1);
            
            /*EditorGUI.BeginChangeCheck();
            brush.Color = EditorGUILayout.ColorField("Color", brush.Color);
            if (EditorGUI.EndChangeCheck())
            {
                Selection.activeGameObject = creator.gameObject;
            }*/
            
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Color");
            GUI.backgroundColor = brush.Color;
            EditorGUILayout.HelpBox("", MessageType.None);
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
        }

        private void DrawTitle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Brush Settings", EditorStyles.boldLabel);

            if (creator.Geomery.Selected.IsRedo && GUILayout.Button("Redo", GUILayout.Width(50)))
                creator.Geomery.Selected.Redo();

            if (creator.Geomery.Selected.IsUndo && GUILayout.Button("Undo", GUILayout.Width(50)))
                creator.Geomery.Selected.Undo();

            GUILayout.EndHorizontal();
        }

        private void DrawBrushBehaviour()
        {
            if (creator.Geomery.Selected == null)
                return;
            
            EditorGUI.BeginChangeCheck();
            creator.Brush.Behaviour = (GeometryBrushBehaviour)EditorGUILayout.EnumPopup("Behaviour", creator.Brush.Behaviour);
            if (EditorGUI.EndChangeCheck())
                EnableBrush();
        }

        private void UpdateBrush()
        {
            if(!brushes.ContainsKey(creator.Brush.Behaviour))
                return;
            var control = brushes[creator.Brush.Behaviour];
            input.Update();
           
            if (input.GetMouseButtonDown())
            {
                EnableBrush();
                control.StartDrawScene();
            }
            if (input.GetMouseButton())
            {
                control.DrawScene();
            }
            if (input.GetMouseButtonUp())
            {
                creator.Geomery.Selected.Record();
                creator.SetDirty();
            }            
        }

        private void DrawSeparator()
        {
            EditorGUILayout.Separator();
        }

        private void EnableBrush()
        {
            if (SceneView.sceneViews.Count > 0)
            {
                var sceneView = (SceneView)SceneView.sceneViews[0];
                sceneView.Focus();
                sceneView.orthographic = true;
            }
        }

        public bool IsGeometryReady()
        {
            return creator.Geomery.Selected != null && creator.Geomery.Selected.Vertices.Count > 0;
        }
        
        public Rect GetWindowRect()
        {
            return new Rect(0, 0, Width, Height);
        }
    }
}
