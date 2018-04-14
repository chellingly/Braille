using Assets.GPUTools.Common.Editor;
using GPUTools.Hair.Scripts.Geometry.Import;
using GPUTools.Skinner.Scripts.Providers;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Import
{
    [CustomEditor(typeof(HairGeometryImporter))]
    public class GeometryImporterEditor : UnityEditor.Editor
    {
        private HairGeometryImporter settings;

        private void OnEnable()
        {
            settings = target as HairGeometryImporter;
        }

        public override void OnInspectorGUI()
        {
            settings.DebugDraw = EditorGUILayout.Toggle("Draw", settings.DebugDraw);

            GUILayout.Label("Bounds", EditorStyles.boldLabel);
            var bounds = settings.Bounds;
            bounds.center = EditorGUILayout.Vector3Field("Center", bounds.center);
            bounds.size = EditorGUILayout.Vector3Field("Size", bounds.size);
            settings.Bounds = bounds;

            GUILayout.Label("Geometry", EditorStyles.boldLabel);
            ScalpProviderInspector();
            settings.Segments = EditorGUILayout.IntSlider("Segments Per Stand", settings.Segments, 3, 25);
            EditorDrawUtils.ListObjectGUI("Hair Mesh Filter", settings.HairGroupsProvider.HairFilters);

            if (GUILayout.Button("Generate Control Stands"))
            {
                settings.Process();

                var sugested = "";
                var count = settings.HairGroupsProvider.Vertices.Count;
                
                for (var i = 3; i < 2000; i++)
                {
                    if ((count % i == 0) && (count / i < 26))
                        sugested += count / i + " ";
                }
                
                if (settings.Indices.Length == 0)
                {
                    Debug.LogWarning("Can't generate control stands. Check if stand roots match scalp vertices. Check if segments field is the same as it was when you've exported geometry, possible are:." + sugested);
                }
            }

        }


        private void ScalpProviderInspector()
        {
            settings.ScalpProvider.Type = (ScalpMeshType)EditorGUILayout.EnumPopup("Scalp Renderer Type", settings.ScalpProvider.Type);

            if (settings.ScalpProvider.Type == ScalpMeshType.Static)
            {
                settings.ScalpProvider.StaticProvider.MeshFilter = (MeshFilter)EditorGUILayout.ObjectField("Scalp Hair Mesh Filter", settings.ScalpProvider.StaticProvider.MeshFilter, typeof(MeshFilter), true);
            }
            else
            {
                settings.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Skinned Mesh Renderer", settings.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer, typeof(SkinnedMeshRenderer), true);
            }
        }
    }
}
