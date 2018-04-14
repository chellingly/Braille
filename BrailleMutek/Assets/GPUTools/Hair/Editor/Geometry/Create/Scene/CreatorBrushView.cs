using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEditor;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Scene
{
    public class CreatorBrushView : EditorItemBase
    {
        protected HairGeometryCreator Creator;

        public CreatorBrushView(HairGeometryCreator creator)
        {
            Creator = creator;
        }

        public override void DrawScene()
        {
            var ray = Camera.current.ScreenPointToRay(GetMousePos());
            var distanceToCamera = Camera.current.transform.InverseTransformPoint(Creator.transform.position).z;

            Creator.Brush.Position = ray.GetPoint(distanceToCamera);
            Creator.Brush.Dirrection = Camera.current.transform.TransformDirection(Vector3.forward);

            if (IsBrushEnabled() && IsGeometryReady())
            {
                DrawBrush();
                EditorUtility.SetDirty(Creator);
            }
        }

        private void DrawBrush()
        {
            var brush = Creator.Brush;

            Handles.color = Color.red;

            var m = Matrix4x4.TRS(Vector3.zero, Quaternion.LookRotation(brush.Dirrection), Vector3.one);

            var step = 2 * Mathf.PI / 20;
            for (var i = 0; i < 20; i++)
            {
                var a = i * step;
                var dir = new Vector3(Mathf.Cos(a), Mathf.Sin(a));
                var dirNext = new Vector3(Mathf.Cos(a + step), Mathf.Sin(a + step));

                var p1 = dir * brush.Radius + Vector3.forward * brush.Lenght1;
                var p1Next = dirNext * brush.Radius + Vector3.forward * brush.Lenght1;

                var p2 = dir * brush.Radius + Vector3.back * brush.Lenght2;
                var p2Next = dirNext * brush.Radius + Vector3.back * brush.Lenght2;

                Handles.DrawLine(brush.ToWorld(m, p1), brush.ToWorld(m, p1Next));
                Handles.DrawLine(brush.ToWorld(m, p2), brush.ToWorld(m, p2Next));
                Handles.DrawLine(brush.ToWorld(m, p1), brush.ToWorld(m, p2));
            }
        }

        private Vector2 GetMousePos()
        {
            var mousePos = Event.current.mousePosition;
            mousePos.y = Camera.current.pixelHeight - mousePos.y;
            return mousePos;
        }

        public bool IsBrushEnabled()
        {
            if (!Application.isPlaying && SceneView.sceneViews.Count > 0 && EditorWindow.focusedWindow != null && Creator.Brush.Behaviour != GeometryBrushBehaviour.None)
            {
                var sceneView = (SceneView)SceneView.sceneViews[0];
                return sceneView.orthographic && sceneView.ToString() == EditorWindow.focusedWindow.ToString();
            }

            return false;
        }
        
        public bool IsGeometryReady()
        {
            return Creator.Geomery.Selected != null && Creator.Geomery.Selected.Vertices.Count > 0;
        }
    }
}
