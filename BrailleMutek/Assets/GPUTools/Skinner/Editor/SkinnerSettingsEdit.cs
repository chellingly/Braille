using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Skinner.Scripts;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;
using UnityEditor;

namespace Assets.GPUTools.Skinner.Editor
{
    [CustomEditor(typeof(SkinnerSettings))]
    public class SkinnerEdit : UnityEditor.Editor {
        private SkinnerSettings settings;
        private Processor processor = new Processor();

        private void OnEnable()
        {
            settings = target as SkinnerSettings;
            if (settings == null)
                return;
        }

        public override void OnInspectorGUI()
        {
            MeshProviderInspector();
            processor.DrawInspector();
        }

        private void OnSceneGUI()
        {
            processor.DrawScene();
        }

        private void OnDisable()
        {
            processor.Dispose();
        }

        #region implementation
        private void MeshProviderInspector()
        {
            settings.DebugDraw = EditorGUILayout.Toggle("Debug Draw", settings.DebugDraw);
            settings.MeshProvider.SkinnedMeshRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Skinned Mesh Renderer", settings.MeshProvider.SkinnedMeshRenderer, typeof(SkinnedMeshRenderer), true);
        }
        #endregion

    }
}
