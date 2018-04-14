using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts;
using GPUTools.Hair.Scripts.Settings;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Settings.Inspector
{
    public class HairStatisticsInspector : EditorItemBase
    {
        private HairSettings settings;
        private bool isVisible;

        public HairStatisticsInspector(HairSettings settings)
        {
            this.settings = settings;
        }

        public override void DrawInspector()
        {
            isVisible = EditorGUILayout.Foldout(isVisible, "Debug");
            if (!isVisible)
                return;
                
            settings.PhysicsSettings.DebugDraw = EditorGUILayout.Toggle("Draw", settings.PhysicsSettings.DebugDraw);
            
            if (Application.isPlaying && StandsSettings.Provider != null && StandsSettings.Provider.GetVertices() != null)
            {

                GUILayout.Label("Physics", EditorStyles.boldLabel);
                GUI.color = Color.gray;
                GUILayout.Label(string.Format("Particles: {0}", StandsSettings.Provider.GetVertices().Count));
                GUILayout.Label(string.Format("Stands: {0}",StandsSettings.Provider.GetStandsNum()));

                GUI.color = Color.white;
                GUILayout.Label("Render", EditorStyles.boldLabel);
                GUI.color = Color.gray;

                var position = StandsSettings.HeadCenterWorld;

                var totalTrianglesInScalp = StandsSettings.Provider.GetIndices().Length / 3f;

                var totalStands = totalTrianglesInScalp*LodSettings.GetDetail(position);
                var totalTrianglesInStand = LodSettings.GetDencity(position)*2;
                var totalTringles = totalTrianglesInStand*totalStands;

                GUILayout.Label(string.Format("Stands: {0}", totalStands));
                GUILayout.Label(string.Format("Polygons per stand: {0}", totalTrianglesInStand));
                GUILayout.Label(string.Format("Polygons: {0}", totalTringles));
                GUI.color = Color.white;
            }
        }

        public HairStandsSettings StandsSettings
        {
            get { return settings.StandsSettings; }
        }

        public HairLODSettings LodSettings
        {
            get { return settings.LODSettings; }
        }
    }
}
