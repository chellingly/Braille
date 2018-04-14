using System.Collections.Generic;
using GPUTools.Common.Scripts.Tools.Ranges;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Common.Editor
{
    public class EditorDrawUtils : MonoBehaviour
    {
        public static void ListObjectGUI<T>(string itemName, List<T> list) where T : Object
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label(itemName + "s", EditorStyles.boldLabel);

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    list[i] = (T)EditorGUILayout.ObjectField(itemName, list[i], typeof(T), true);

                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
                    {
                        list.RemoveAt(i);
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.BeginHorizontal();

            GUILayout.Label(list == null || list.Count == 0 ? "Add " + itemName : "");
            if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
            {
                if (list != null) list.Add(default(T));
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        public static void ListColorGUI(string itemName, List<Color> list)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label(itemName + "s", EditorStyles.boldLabel);

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    list[i] = EditorGUILayout.ColorField(itemName, list[i]);

                    if (GUILayout.Button("-", EditorStyles.miniButton, GUILayout.MaxWidth(20)))
                    {
                        list.RemoveAt(i);
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.BeginHorizontal();

            GUILayout.Label(list == null || list.Count == 0 ? "Add " + itemName : "");
            if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
            {
                if (list != null) list.Add(new Color());
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
        
        public static void Warning(string str)
        {
            GUI.color = Color.grey;
            EditorGUILayout.LabelField(str);
            GUI.color = Color.white;
        }

        public static void Folder(string name, ref bool show, ref bool enabled)
        {
            EditorGUILayout.BeginHorizontal();
            show = EditorGUILayout.Foldout(show, name);

            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            enabled = EditorGUILayout.Toggle("", enabled);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }
        
        public static FloatRange Range(string name, FloatRange range, float min, float max, int round = 3)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.LabelField(name + " [Min:" + Round(range.Min,round) + " Max:" + Round(range.Max,round) + "]");
            EditorGUILayout.MinMaxSlider("", ref range.Min, ref range.Max, min,max);
            EditorGUILayout.EndVertical();
            return range;
        }
        
        public static float Round(float value, int digits)
        {
            var mult = Mathf.Pow(10.0f, digits);
            return Mathf.Round(value * mult) / mult;
        }
       
    }
}
