using GPUTools.Hair.Scripts.Utils;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Debug
{
    public class MayaImporterDebugDraw
    {
        public static void Draw(MayaHairGeometryImporter importer)
        {
            Gizmos.color = Color.green;

            var vertices = importer.Data.Vertices;
            var matrix = importer.ScalpProvider.ToWorldMatrix;

            for (var i = 1; i < vertices.Count; i++)
            {
                if(i % importer.Data.Segments == 0)
                    continue;

                var vertex0 = matrix.MultiplyPoint3x4(vertices[i - 1]);
                var vertex1 = matrix.MultiplyPoint3x4(vertices[i]);

                Gizmos.DrawLine(vertex0, vertex1);
            }

            var worldBounds = importer.GetBounds();
            Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
            
            /*for (var i = 0; i < mayaVertices.Count; i++)
            {
                Gizmos.DrawWireSphere(matrix.MultiplyPoint3x4(mayaVertices[i]), 0.0002f);
            }*/
        }
    }
}
