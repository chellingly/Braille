using GPUTools.Hair.Scripts.Geometry.MayaImport;
using GPUTools.Skinner.Scripts;
using GPUTools.Skinner.Scripts.Providers;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Import
{
    [CustomEditor(typeof(MayaHairGeometryImporter))]
    public class MayaGeometryImporterEditor : UnityEditor.Editor
    {
        private MayaHairGeometryImporter settings;

        private void OnEnable()
        {
            settings = target as MayaHairGeometryImporter;
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
            HairMeshFiltersList();

            settings.RegionThresholdDistance =
                EditorGUILayout.FloatField("Region threshold", settings.RegionThresholdDistance);
            if (GUILayout.Button("Generate Stands"))
            {
                settings.Process();
            }
        }

        private void ScalpProviderInspector()
        {
            settings.ScalpProvider.Type = (ScalpMeshType)EditorGUILayout.EnumPopup("Scalp Renderer Type", settings.ScalpProvider.Type);

            if (settings.ScalpProvider.Type == ScalpMeshType.Static)
            {
                settings.ScalpProvider.StaticProvider.MeshFilter = (MeshFilter)EditorGUILayout.ObjectField("Scalp Mesh Filter", settings.ScalpProvider.StaticProvider.MeshFilter, typeof(MeshFilter), true);
            }
            else
            {
                settings.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Skinned Mesh Renderer", settings.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer, typeof(SkinnedMeshRenderer), true);
            }
        }

        private void HairMeshFiltersList()
        {
            settings.HairContainer = (GameObject)EditorGUILayout.ObjectField("Hair Container", settings.HairContainer, typeof(GameObject), true);
        }
    }
}
