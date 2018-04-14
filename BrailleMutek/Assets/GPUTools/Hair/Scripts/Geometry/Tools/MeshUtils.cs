using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Tools
{
    public class MeshUtils
    {
        public static List<Vector3> GetWorldVertices(MeshFilter fiter)
        {
            var list = new List<Vector3>();
            var vertices = fiter.sharedMesh.vertices;

            foreach (var vertex in vertices)
            {
                list.Add(fiter.transform.TransformPoint(vertex));    
            }

            return list;
        }

        public static List<Vector3> GetVerticesInSpace(Mesh mesh, Matrix4x4 toWorld, Matrix4x4 toTransform)
        {
            var list = new List<Vector3>();

            for (var i = 0; i < mesh.vertexCount; i++)
            {
                var vertex = mesh.vertices[i];
                var worldVertex = toWorld.MultiplyPoint3x4(vertex);
                var spaceVertex = toTransform.MultiplyPoint3x4(worldVertex);
                list.Add(spaceVertex);
            }

            return list;
        }

    }
}
