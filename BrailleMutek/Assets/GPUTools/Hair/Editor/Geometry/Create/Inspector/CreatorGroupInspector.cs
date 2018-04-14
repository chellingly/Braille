using System.Collections.Generic;
using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Geometry.Create;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Inspector
{
    public class CreatorGroupInspector : EditorItemBase
    {
        private HairGeometryCreator creator;

        public CreatorGroupInspector(HairGeometryCreator creator)
        {
            this.creator = creator;
        }

        private GeometryGroupData newData;

        public override void DrawInspector()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("Hair Groups", EditorStyles.boldLabel);
            DrawGroupsList();
            GUILayout.EndVertical();

            if (GUILayout.Button("Optimize"))
            {
                creator.Optimize();
            }
        }

        private void DrawGroupsList()
        {
            var geomery = creator.Geomery;
            if (geomery != null)
            {
                for (int i = 0; i < geomery.List.Count; i++)
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    if (creator.Geomery.SelectedIndex == i)
                    {
                        DrawSelectedGroup(geomery.List, i);
                    }
                    else
                    {
                        DrawGroupButton(geomery.List, i);
                    }
                                        
                    GUILayout.EndVertical();
                }
                
                if(newData != null)
                    DrawNewGeometry();
            }

            if (newData == null)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(geomery == null || geomery.List.Count == 0 ? "Add Group" : "");

                if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
                {
                    newData = new GeometryGroupData();
                }

                GUILayout.EndHorizontal();
            }
        }

        private void DrawNewGeometry()
        {
            EditorGUILayout.BeginHorizontal();
            newData.Length = Mathf.Max(EditorGUILayout.FloatField("Length", newData.Length), 0);

            if (GUILayout.Button("Create",EditorStyles.miniButton, GUILayout.Width(60)))
            {
                newData.Generate(creator.ScalpProvider.Mesh,  creator.Segments);
                
                creator.Geomery.List.Add(newData);
                creator.Geomery.SelectedIndex = creator.Geomery.List.Count - 1;
                
                creator.SetDirty();

                newData = null;
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawSelectedGroup(List<GeometryGroupData> list, int i)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("< Geometry Group " + i);
            if (GUILayout.Button("edit", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxWidth(40)))
            {}

            if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
            {
                list.RemoveAt(i);
                creator.Geomery.SelectedIndex = 0;
                creator.SetDirty();
            }
            GUILayout.EndHorizontal();

            
        }

        private void DrawGroupButton(List<GeometryGroupData> list, int i)
        {
            GUILayout.BeginHorizontal();


            GUILayout.Label("Geometry Group " + i);

            if (GUILayout.Button("edit", EditorStyles.miniButton, GUILayout.MaxWidth(40)))
                creator.Geomery.SelectedIndex = i;

            if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
            {
                list.RemoveAt(i);
                creator.Geomery.SelectedIndex = 0;
                creator.SetDirty();
            }

            GUILayout.EndHorizontal();
        }
    }
}
