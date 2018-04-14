using System.Linq;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.Tools;
using GPUTools.Skinner.Scripts;
using GPUTools.Skinner.Scripts.Providers;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
    public class ComputeStandsToScalpVerticesMap : CacheChainCommand
    {
        private readonly MayaHairGeometryImporter importer;

        public ComputeStandsToScalpVerticesMap(MayaHairGeometryImporter importer)
        {
            this.importer = importer;
        }

        protected override void OnCache()
        {
            importer.Data.HairRootToScalpMap = ProcessHairRootToScalpMap();
        }

        private int[] ProcessHairRootToScalpMap()
        {
            if (importer.ScalpProvider.Type == ScalpMeshType.Skinned)
            {
                var scalpVertices = importer.ScalpProvider.Mesh.vertices.ToList();
                return ScalpProcessingTools.HairRootToScalpIndices(scalpVertices, importer.Data.Vertices, importer.Data.Segments).ToArray();
            }

            return new int[importer.Data.Vertices.Count / importer.Data.Segments];
        }
    }
}
