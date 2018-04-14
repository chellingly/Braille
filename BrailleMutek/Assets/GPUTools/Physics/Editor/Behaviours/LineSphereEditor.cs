using GPUTools.Physics.Scripts.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Physics.Editor.Behaviours
{
    [CustomEditor(typeof(LineSphereCollider))]
    public class LineSphereEditor : UnityEditor.Editor
    {
        private LineSphereCollider collider;
        private bool EditA;
        private bool EditB;


        private void OnEnable()
        {
            collider = target as LineSphereCollider;

        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            collider.A = EditorGUILayout.Vector3Field("A", collider.A);
            EditorGUI.BeginDisabledGroup(EditA);
            if (GUILayout.Button("Edit", EditorStyles.miniButton, GUILayout.MaxWidth(30)))
            {
                EditA = true;
                EditB = false;
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            collider.B = EditorGUILayout.Vector3Field("B", collider.B);
            EditorGUI.BeginDisabledGroup(EditB);
            if (GUILayout.Button("Edit", EditorStyles.miniButton, GUILayout.MaxWidth(30)))
            {
                EditB = true;
                EditA = false;
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            collider.RadiusA = EditorGUILayout.FloatField("Radius A", collider.RadiusA);
            collider.RadiusB = EditorGUILayout.FloatField("Radius B", collider.RadiusB);

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

        private void OnSceneGUI()
        {
            if(EditA)
                collider.WorldA = Handles.PositionHandle(collider.WorldA, Quaternion.identity);

            if(EditB)
                collider.WorldB = Handles.PositionHandle(collider.WorldB, Quaternion.identity);
        }
    }
}
