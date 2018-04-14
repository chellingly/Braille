using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
    [Serializable]
    public class GeometryGroupData
    {
        public float Length = 2;
        public int Segments;

        public List<Vector3> GuideVertices;
        public List<float> Distances; 
        public List<Vector3> Vertices;
        public List<Color> Colors;

        [SerializeField] private GeometryGroupHistory history = new GeometryGroupHistory();
        
        public void Generate(Mesh mesh, int segments)
        {
            Vertices = new List<Vector3>();
            GuideVertices = new List<Vector3>();
            Distances = new List<float>();
            
            var firstVertices = new List<Vector3>();

            for (var i = 0; i < mesh.vertices.Length; i++)
            {
                var vertex = mesh.vertices[i];
                var normal = mesh.normals[i];

                if (firstVertices.Contains(vertex))
                    continue;

                var stand = CreateStand(vertex, normal, segments);
                Vertices.AddRange(stand);
                Distances.Add(Length/segments);
                GuideVertices.AddRange(stand);
                firstVertices.Add(vertex);
            }

            Colors  = new List<Color>();
            for (var i = 0; i < Vertices.Count; i++)
            {
                Colors.Add(new Color(1,1,1));
            }

            Debug.Log("Total nodes:" + Vertices.Count);
        }

        public void Fix()
        {

        }

        public void Reset()
        {
            Vertices.Clear();
            Vertices = null;
        }

        private List<Vector3> CreateStand(Vector3 start, Vector3 normal, int segments)
        {
            var list = new List<Vector3>();

            var step = Length/segments;
            for (var i = 0; i < segments; i++)
            {
                list.Add(start + normal*(step*i));
            }

            return list;
        }

        #region History

        public void Record()
        {
            history.Record(Vertices);
        }
        
        public void Undo()
        {
            if (history.IsUndo)
                Vertices = history.Undo();
        }

        public void Redo()
        {
            if (history.IsRedo)
                Vertices = history.Redo();
        }

        public bool IsUndo
        {
            get { return history.IsUndo; }
        }

        public bool IsRedo
        {
            get { return history.IsRedo; }
        }

        public void Clear()
        {
            history.Clear();
        }

        #endregion

        public void OnDrawGizmos(int segments, bool isSelected, Matrix4x4 toWorld)
        {
            Segments = segments;
            if (Vertices == null)
                return;

            if (Colors == null || Colors.Count != Vertices.Count)
                FillColors();

            for (var i = 1; i < Vertices.Count; i++)
            {
                if (i%segments == 0)
                    continue;

                var color = Colors[i];
                Gizmos.color = isSelected ? color : Color.grey;

                var vertex1 = toWorld.MultiplyPoint3x4(Vertices[i - 1]);
                var vertex2 = toWorld.MultiplyPoint3x4(Vertices[i]);

                Gizmos.DrawLine(vertex1, vertex2);
            }
        }

        private void FillColors()
        {
            Colors = new List<Color>();
            for (var i = 0; i < Vertices.Count; i++)
                Colors.Add(Color.white);
        }
    }
}
