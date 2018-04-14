using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Commands;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport
{
    public class CacheMayaHairData : CacheChainCommand
    {
        private readonly MayaHairGeometryImporter importer;

        public CacheMayaHairData(MayaHairGeometryImporter importer)
        {
            this.importer = importer;
            Add(new StripsToLinesMayaPass(importer));
            Add(new AssignHairLinesToTriangles(importer));
            Add(new MoveHairLinesToScalpVertices(importer));
            Add(new ComputeStandsToScalpVerticesMap(importer));
        }

        protected override void OnCache()
        {
            base.OnCache();

            UnityEngine.Debug.Log("segments:" + importer.Data.Segments);
            UnityEngine.Debug.Log("vertices:" + importer.Data.Vertices.Count);
            UnityEngine.Debug.Log("stands:" + importer.Data.Vertices.Count / importer.Data.Segments);
            UnityEngine.Debug.Log("scalp triangles:" + importer.Data.Indices.Length / 3);
        }
    }
}
