using GPUTools.Hair.Scripts.Geometry.Procedural;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Procedural
{
    [CustomEditor(typeof(ProceduralScalp))]
    public class ProceduralScalpEdit : UnityEditor.Editor
    {
        private ProceduralScalp settings;

        private void OnEnable()
        {
            settings = target as ProceduralScalp;
        }

        private int selectedX;
        private int selectedY;

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            settings.Grid.ControlSizeX = EditorGUILayout.IntSlider("ControlSizeX", settings.Grid.ControlSizeX, 3, 6);
            settings.Grid.ControlSizeY = EditorGUILayout.IntSlider("ControlSizeY", settings.Grid.ControlSizeY, 3, 6);

            if (GUILayout.Button("GenerateControl"))
            {
                settings.Grid.GenerateControl();
            }

            settings.Grid.ViewSizeX = EditorGUILayout.IntSlider("ControlSizeX", settings.Grid.ViewSizeX, 3, 12);
            settings.Grid.ViewSizeY = EditorGUILayout.IntSlider("ControlSizeY", settings.Grid.ViewSizeY, 3, 12);

            GUILayout.EndVertical();
        }

        private void OnSceneGUI()
        {
            Handles.color = Color.red;

            SelectPoint();
            EditPoint();
        }

        private void SelectPoint()
        {
            for (var x = 0; x < settings.Grid.ControlSizeX; x++)
            {
                for (var y = 0; y < settings.Grid.ControlSizeY; y++)
                {
                    var point = settings.Grid.GetControl(x, y);
                    var p = settings.transform.TransformPoint(point);
                    var size = HandleUtility.GetHandleSize(p) * 0.05f;

                    if (Handles.Button(p, Quaternion.identity, size, size, Handles.DotHandleCap))
                    {
                        selectedX = x;
                        selectedY = y;
                    }
                }
            }
        }

        private void EditPoint()
        {
            var point = settings.Grid.GetControl(selectedX, selectedY);
            var p = settings.transform.TransformPoint(point);

            EditorGUI.BeginChangeCheck();
            p = Handles.DoPositionHandle(p, settings.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                settings.Grid.SetControl(selectedX, selectedY, settings.transform.InverseTransformPoint(p));
            }
        }
    }
}
