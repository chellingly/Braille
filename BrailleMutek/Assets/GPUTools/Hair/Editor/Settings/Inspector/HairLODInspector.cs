using Assets.GPUTools.Common.Editor;
using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts;
using GPUTools.Hair.Scripts.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.GPUTools.Hair.Editor.Settings.Inspector
{
    public class HairLODInspector : EditorItemBase
    {
        private HairSettings settings;

        public HairLODInspector(HairSettings settings)
        {
            this.settings = settings;
        }

        public override void DrawInspector()
        {
            Lod.IsVisible = EditorGUILayout.Foldout(Lod.IsVisible, "LOD");
            if (!Lod.IsVisible)
                return;
           
            Lod.ViewCamera = (Camera)EditorGUILayout.ObjectField("Camera (Optional):", Lod.ViewCamera, typeof(Camera), true);
            
            var center = Geometry.HeadCenterWorld;
            var distance = EditorDrawUtils.Round(Lod.GetDistanceToCamera(center), 2);
            var thickness =  EditorDrawUtils.Round(Lod.GetWidth(center), 6);
            var density = EditorDrawUtils.Round(Lod.GetDencity(center), 1);
            var detail = EditorDrawUtils.Round(Lod.GetDetail(center), 1);
            
            Lod.Distance = EditorDrawUtils.Range("Distance:" + distance, Lod.Distance, 0, 50);
            Lod.Width = EditorDrawUtils.Range("Thickness:" + thickness, Lod.Width, 0, 0.01f, 6);
            Lod.Density = EditorDrawUtils.Range("Density:" + density, Lod.Density, 4, 64);
            Lod.Detail = EditorDrawUtils.Range("Detail:" + detail, Lod.Detail, 4, 64);
        }

        public HairLODSettings Lod
        {
            get { return settings.LODSettings; }
        }
        
        public HairStandsSettings Geometry
        {
            get { return settings.StandsSettings; }
        }
    }
}
