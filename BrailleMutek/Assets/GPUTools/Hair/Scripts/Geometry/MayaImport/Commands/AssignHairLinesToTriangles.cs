using System.Collections.Generic;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Common.Scripts.Utils;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Data;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
    public class AssignHairLinesToTriangles : ICacheCommand
    {
        private readonly MayaHairGeometryImporter importer;
        private readonly MayaHairData data;

        public AssignHairLinesToTriangles(MayaHairGeometryImporter importer)
        {
            this.importer = importer;
            data = importer.Data;
        }
        
        public void Cache()
        {
            data.TringlesCenters = ComputeTringlesCenters();

            data.Lines = Assign(data.TringlesCenters);
        }

        private List<Vector3> Assign(List<Vector3> scalpTringlesCenters)
        {
            var oldVertices = data.Lines;
            var newVertices = new List<Vector3>();
            var set = new List<Vector3>();

            for (var i = 0; i < oldVertices.Count; i += data.Segments)
            {
                var vertex = oldVertices[i];
                var closeScalpVertex = MathSearchUtils.FindCloseVertex(scalpTringlesCenters, vertex);
                var offset = closeScalpVertex - vertex;
                
                if (!set.Contains(closeScalpVertex))
                {
                    var newStand = CreateStandWithOffsetForRegion(oldVertices, offset, i, i + data.Segments);
                    newVertices.AddRange(newStand);
                    set.Add(closeScalpVertex);
                }
            }

            return newVertices;
        }


        private List<Vector3> ComputeTringlesCenters()
        {
            var scalpIndices = importer.ScalpProvider.Mesh.GetIndices(0);
            var scalpVertices = importer.ScalpProvider.Mesh.vertices;

            var centers = new List<Vector3>();

            for (var i = 0; i < scalpIndices.Length; i+=3)
            {
                var v1 = scalpVertices[scalpIndices[i]];
                var v2 = scalpVertices[scalpIndices[i + 1]];
                var v3 = scalpVertices[scalpIndices[i + 2]];

                centers.Add((v1 + v2 + v3)/3);
            }

            return centers;
        }

        private List<Vector3> CreateStandWithOffsetForRegion(List<Vector3> vertices, Vector3 offset, int start, int end)
        {
            var result = new List<Vector3>();
            for (var i = start; i < end; i++)
            {
                result.Add(vertices[i] + offset);
            }
            return result;
        }
    }
}
