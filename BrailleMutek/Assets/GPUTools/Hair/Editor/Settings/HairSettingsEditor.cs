using Assets.GPUTools.Common.Editor.Engine;
using Assets.GPUTools.Hair.Editor.Settings.Inspector;
using Assets.GPUTools.Hair.Editor.Settings.Scene;
using GPUTools.Hair.Scripts;
using GPUTools.Hair.Scripts.Settings;
using UnityEditor;

// ReSharper disable PossibleNullReferenceException

namespace Assets.GPUTools.Hair.Editor.Settings
{
    [CustomEditor(typeof(HairSettings))]
    public class HairSettingsEditor : UnityEditor.Editor
    {
        private HairSettings settings;
        private Processor processor = new Processor();

        private void OnEnable()
        {
            settings = target as HairSettings;

            processor.Add(new HairStandsEditor(settings));

            processor.Add(new HairStandsInspector(settings));
            processor.Add(new HairPhysicsInspector(settings));
            processor.Add(new HairRenderInspector(settings));
            processor.Add(new HairShadowInspector(settings));
            processor.Add(new HairLODInspector(settings));
            processor.Add(new HairStatisticsInspector(settings));
        }

        public override void OnInspectorGUI()
        {
            
            processor.DrawInspector();
        }

        private void OnSceneGUI()
        {
            processor.DrawScene();
        }
    }
}
