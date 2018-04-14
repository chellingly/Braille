using System.Collections.Generic;
using Assets.GPUTools.Common.Editor.Engine;
using Assets.GPUTools.Hair.Editor.Geometry.Create.Inspector;
using Assets.GPUTools.Hair.Editor.Geometry.Create.Scene;
using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create
{
    [CustomEditor(typeof(HairGeometryCreator))]
    public class HairGeometryCreatorEditor : UnityEditor.Editor
    {
        private Processor processor = new Processor();
        private HairGeometryCreator creator;

        private void OnEnable()
        {
            creator = target as HairGeometryCreator;
            if(creator == null)
                return;

            processor.Add(new CreatorInputInspector(creator));
            processor.Add(new CreatorGroupInspector(creator));
            processor.Add(new CreatorBrushInspector(creator));
            processor.Add(new CreatorBrushView(creator));
        }

        public override void OnInspectorGUI()
        {
            processor.DrawInspector();
        }

        private void OnSceneGUI()
        {
            if(creator == null)
                return;

            processor.DrawScene();
        }

        private void OnDisable()
        {
            processor.Dispose();
        }
    }
}
