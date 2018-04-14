using System.Collections.Generic;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Data;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
    public class MoveHairLinesToScalpVertices : ICacheCommand
    {
        private readonly MayaHairGeometryImporter importer;
        private readonly MayaHairData data;

        public MoveHairLinesToScalpVertices(MayaHairGeometryImporter importer)
        {
            this.importer = importer;
            data = importer.Data;
        }

        public void Cache()
        {
            var scalpIndices = importer.ScalpProvider.Mesh.GetIndices(0);
            var scalpVertices = importer.ScalpProvider.Mesh.vertices;
            var hairVertices = new List<Vector3>();

            var indices = new List<int>();

            for (var i = 0; i < scalpIndices.Length; i += 3)
            {
                var center = data.TringlesCenters[i / 3];
                var stand = FindStandIndex(data.Lines, center, data.Segments);
                if(stand == -1)
                    continue;

                for (var j = 0; j < 3; j++)
                {
                    var scalpVertex = scalpVertices[scalpIndices[i + j]];
                    var offset = scalpVertex - center;

                    var hairStand = FindStandIndex(hairVertices, scalpVertex, data.Segments);
                    if (hairStand == -1 || !CompareStands(hairVertices, hairStand, data.Lines, stand, data.Segments))
                    {
                       
                        indices.Add(hairVertices.Count/data.Segments);
                        hairVertices.AddRange(CreateStandWithOffsetForRegion(data.Lines, offset, stand, data.Segments));
                    }
                    else
                    {
                        indices.Add(hairStand/data.Segments);
                    }
                }
            }

            data.Vertices = hairVertices;
            data.Indices = indices.ToArray(); 
        }

        private bool CompareStands(List<Vector3> hairStands1, int stand1, List<Vector3> hairStands2, int stand2, int segments)
        {
            var sqrLimit = importer.RegionThresholdDistance * importer.RegionThresholdDistance;

            for (var i = 0; i < segments; i++)
            {
                var v1 = hairStands1[stand1 + i];
                var v2 = hairStands2[stand2 + i];

                if ((v1 - v2).sqrMagnitude > sqrLimit)
                    return false;
            }

            return true;
        }

        private int FindStandIndex(List<Vector3> hairVertices, Vector3 vertex, int segments)
        {
            for (var i = 0; i < hairVertices.Count; i += segments)
                if (hairVertices[i] == vertex)
                    return i;

            return -1;
        }

        private List<Vector3> CreateStandWithOffsetForRegion(List<Vector3> vertices, Vector3 offset, int start, int segments)
        {
            var result = new List<Vector3>();
            var end = start + segments;

            for (var i = start; i < end; i++)
            {
                result.Add(vertices[i] + offset);
            }
            return result;
        }
    }
}
