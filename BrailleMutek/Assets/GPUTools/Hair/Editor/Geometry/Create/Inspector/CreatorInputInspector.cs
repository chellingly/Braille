using Assets.GPUTools.Common.Editor;
using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Geometry.Create;
using GPUTools.Skinner.Scripts;
using GPUTools.Skinner.Scripts.Providers;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Inspector
{
    public class CreatorInputInspector : EditorItemBase
    {
        private HairGeometryCreator creator;

        public CreatorInputInspector(HairGeometryCreator creator)
        {
            this.creator = creator;
        }

        public override bool Validate()
        {
            if (creator.ScalpProvider == null)
                return false;

            var isStatic = creator.ScalpProvider.Type == ScalpMeshType.Static && creator.ScalpProvider.StaticProvider.MeshFilter != null;
            var isSkinned = creator.ScalpProvider.Type == ScalpMeshType.Skinned && creator.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer != null;

            return isStatic || isSkinned;
        }

        public override void DrawInspector()
        {
            creator.DebugDraw = EditorGUILayout.Toggle("Draw", creator.DebugDraw);
            
            GUILayout.Label("Bounds", EditorStyles.boldLabel);
            var bounds = creator.Bounds;
            bounds.center = EditorGUILayout.Vector3Field("Center", bounds.center);
            bounds.size = EditorGUILayout.Vector3Field("Size", bounds.size);
            creator.Bounds = bounds;
            
            GUILayout.Label("Source", EditorStyles.boldLabel);

            if (creator.Geomery.Selected == null)
            {
                creator.Segments = Mathf.Clamp(EditorGUILayout.IntField("Segments", creator.Segments), 3, 30);
            }
            else
            {
                GUILayout.Label("Segments " + creator.Segments);
            }

            ScalpProviderInspactor();
            EditorDrawUtils.ListObjectGUI("Collider", creator.ColliderProviders);
        }

        private void ScalpProviderInspactor()
        {
            creator.ScalpProvider.Type = (ScalpMeshType)EditorGUILayout.EnumPopup("Scalp Renderer Type", creator.ScalpProvider.Type);

            if (creator.ScalpProvider.Type == ScalpMeshType.Static)
            {
                creator.ScalpProvider.StaticProvider.MeshFilter = (MeshFilter)EditorGUILayout.ObjectField("Scalp", creator.ScalpProvider.StaticProvider.MeshFilter, typeof(MeshFilter), true);
            }
            else
            {
                creator.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("Skinned Mesh Renderer", creator.ScalpProvider.SkinnedProvider.SkinnedMeshRenderer, typeof(SkinnedMeshRenderer), true);
            }
        }
    }
}
