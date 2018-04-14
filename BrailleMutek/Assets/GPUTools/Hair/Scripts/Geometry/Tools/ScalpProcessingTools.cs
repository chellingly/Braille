using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Hair.Scripts.Geometry.Tools
{
    public class ScalpProcessingTools
    {
        public const float Accuracy = 0.00001f;
        
        public static List<int> HairRootToScalpIndices(List<Vector3> scalpVertices,
            List<Vector3> hairVertices, int segments, float accuracy = Accuracy)
        {
            var resultIndices = new List<int>();

            for (var i = 0; i < hairVertices.Count; i += segments)
            {
                for (var j = 0; j < scalpVertices.Count; j++)
                {
                    if ((hairVertices[i] - scalpVertices[j]).sqrMagnitude < accuracy*accuracy)
                    {
                        resultIndices.Add(j);
                        break;
                    }
                }
            }

            Assert.IsTrue(resultIndices.Count == hairVertices.Count/segments, "Hair geometry is not compatible with scalp");
            return resultIndices;
        }


        public static List<int> ProcessIndices(List<int> scalpIndices, List<Vector3> scalpVertices, List<List<Vector3>> hairVerticesGroups, int segments, float accuracy = Accuracy)
        {
            var hairIndices = new List<int>();

            var grouStartIndex = 0;
            foreach (var hairVertices in hairVerticesGroups)
            {
                var groupIndices = ProcessIndicesForMesh(grouStartIndex, scalpVertices, scalpIndices, hairVertices, segments, accuracy);
                hairIndices.AddRange(groupIndices);

                grouStartIndex += hairVertices.Count;
            }

            for (var i = 0; i < hairIndices.Count; i++)
            {
                hairIndices[i] = hairIndices[i] / segments;
            }

            return hairIndices;
        }

        private static List<int> ProcessIndicesForMesh(int startIndex, List<Vector3> scalpVertices, List<int> scalpIndices, List<Vector3> hairVertices, int segments, float accuracy = Accuracy)
        {
            var hairIndices = new List<int>();

            for (var i = 0; i < scalpIndices.Count; i++)
            {
                var index = scalpIndices[i];
                var scalpVertex = scalpVertices[index];

                if (i % 3 == 0)
                    FixNotCompletedPolygon(hairIndices);

                for (var j = 0; j < hairVertices.Count; j += segments)
                {
                    var hairVertex = hairVertices[j];

                    if ((hairVertex - scalpVertex).sqrMagnitude < accuracy*accuracy)
                    {
                        hairIndices.Add(startIndex + j);
                        break;
                    }
                }
            }

            FixNotCompletedPolygon(hairIndices);
            return hairIndices;
        }

        private static void FixNotCompletedPolygon(List<int> hairIndices)
        {
            var countToRemove = hairIndices.Count % 3;
            if (countToRemove > 0)
                hairIndices.RemoveRange(hairIndices.Count - countToRemove, countToRemove);
        }

        public static float MiddleDistanceBetweenPoints(Mesh mesh)
        {
            var vertices = mesh.vertices;
            var indices = mesh.GetIndices(0);

            var sum = 0f;
            var count = 0;

            for (var i = 0; i < Mathf.Min(500, indices.Length); i += 3)
            {
                var v0 = vertices[indices[i]];
                var v1 = vertices[indices[i + 1]];

                sum += Vector3.Distance(v0, v1);
                count++;
            }
            return sum / count;
        }

        public static List<Vector3> ShiftToScalpRoot(List<Vector3> scalpVertices, List<Vector3> hairVertices, int segments)
        {
            for (var i = 0; i < hairVertices.Count; i += segments)
            {
                var minScalpIndex = 0;
                var minDistance = float.MaxValue;
                
                for (var j = 0; j < scalpVertices.Count; j++)
                {
                    var distance = (hairVertices[i] - scalpVertices[j]).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        minScalpIndex = j;
                        minDistance = distance;
                    }
                }

                hairVertices[i] = scalpVertices[minScalpIndex];
            }

            return hairVertices;
        }
    }
}
