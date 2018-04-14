using System.Collections.Generic;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
    public class StripsToLinesMayaPass : ICacheCommand
    {
        private readonly MayaHairGeometryImporter importer;

        public StripsToLinesMayaPass(MayaHairGeometryImporter importer)
        {
            this.importer = importer;
        }

        public void Cache()
        {
            var meshFilters = importer.HairContainer.GetComponentsInChildren<MeshFilter>();
            var mayaHairVertices = new List<Vector3>();

            for (var i = 0; i < meshFilters.Length; i++)
            {
                var stand = CacheStand(meshFilters[i]);
                mayaHairVertices.AddRange(stand);
                importer.Data.Segments = stand.Count;
            }

            importer.Data.Lines = mayaHairVertices;
        }

        private List<Vector3> CacheStand(MeshFilter meshFilter)
        {
            var stripVertices = meshFilter.sharedMesh.vertices;
            var stripTriangles = meshFilter.sharedMesh.triangles;

            var lineVertices = new List<Vector3>();

            for (var i = 0; i < stripTriangles.Length; i += 6)
            {
                var index = stripTriangles[i + 2];
                var vertex = ToScalpSpace(meshFilter, stripVertices[index]);
                lineVertices.Add(vertex);
            }

            return lineVertices;
        }

        private Vector3 ToScalpSpace(MeshFilter filter, Vector3 point)
        {
            var worldToScalpMatrix = importer.ScalpProvider.ToWorldMatrix.inverse;
            var worldPoint = filter.transform.TransformPoint(point);

            return worldToScalpMatrix.MultiplyPoint3x4(worldPoint);
        }
    }
}
