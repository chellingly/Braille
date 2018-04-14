using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
    public class MathSearchUtils
    {
        public static List<Vector3> FindCloseVertices(Vector3[] vertices, Vector3 testVertex, int count)
        {
            var result = new List<Vector3>();
            for (var i = 0; i < count; i++)
            {
                var closeVertex = FindCloseVertex(vertices, testVertex, result);
                result.Add(closeVertex);
            }

            return result;
        }

        public static Vector3 FindCloseVertex(Vector3[] vertices, Vector3 testVertex, List<Vector3> ignoreList = null)
        {
            var resultSqrDistance = float.PositiveInfinity;
            var resultVertex = vertices[0];

            foreach (var vertex in vertices)
            {
                if(ignoreList != null && ignoreList.Contains(vertex))
                    continue;

                var sqrDistance = (vertex - testVertex).sqrMagnitude;
                if (sqrDistance < resultSqrDistance)
                {
                    resultSqrDistance = sqrDistance;
                    resultVertex = vertex;
                }
            }

            return resultVertex;
        }

        public static Vector3 FindCloseVertex(List<Vector3> vertices, Vector3 testVertex, List<Vector3> ignoreList = null)
        {
            var resultSqrDistance = float.PositiveInfinity;
            var resultVertex = vertices[0];

            foreach (var vertex in vertices)
            {
                if (ignoreList != null && ignoreList.Contains(vertex))
                    continue;

                var sqrDistance = (vertex - testVertex).sqrMagnitude;
                if (sqrDistance < resultSqrDistance)
                {
                    resultSqrDistance = sqrDistance;
                    resultVertex = vertex;
                }
            }

            return resultVertex;
        }
    }
}
