using System.Collections.Generic;
using System.Linq;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.Tools;
using GPUTools.Hair.Scripts.Utils;
using GPUTools.Skinner.Scripts;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

#pragma warning disable 649

namespace GPUTools.Hair.Scripts.Geometry.Import
{
    [ExecuteInEditMode]
    public class HairGeometryImporter : GeometryProviderBase
    {
        [SerializeField] public bool DebugDraw = true;

        [SerializeField] public int Segments = 5;
        [SerializeField] public HairGroupsProvider HairGroupsProvider = new HairGroupsProvider();
        [SerializeField] public MeshProvider ScalpProvider = new MeshProvider();

        [SerializeField] public int[] Indices;

        [SerializeField] public Bounds Bounds;

        public override bool Validate(bool log)
        {
            if (Indices == null || Indices.Length == 0)
            {
                if(log)
                    Debug.LogError("Provider does not have any generated hair geometry");
                return false;
            }    
            
            return ValidateImpl(log);
        }

        private bool ValidateImpl(bool log)
        {
            if (!ScalpProvider.Validate(false))
            {
                if(log)
                    Debug.LogError("Scalp field is null");
                
                return false;
            }

            return HairGroupsProvider.Validate(log);
        }

        public void Process()
        {
            if(!ValidateImpl(true))
                return;
       
            HairGroupsProvider.Process(ScalpProvider.ToWorldMatrix.inverse);    
            
            Indices = ProcessIndices();
        }

        public override void Dispatch()
        {
            ScalpProvider.Dispatch();
        }

        private void OnDestroy()
        {
            ScalpProvider.Dispose();
        }

        private int[] ProcessMap()
        {
            var accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(ScalpProvider.Mesh)*0.1f;
            if (ScalpProvider.Type == ScalpMeshType.Skinned)
            {
                var scalpVertices = ScalpProvider.Mesh.vertices.ToList();
                return ScalpProcessingTools.HairRootToScalpIndices(scalpVertices, HairGroupsProvider.Vertices, GetSegmentsNum(), accuracy).ToArray();
            }

            return new int[HairGroupsProvider.Vertices.Count/GetSegmentsNum()];
        }

        private int[] ProcessIndices()
        {
            var accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(ScalpProvider.Mesh)*0.1f;
            var scalpIndices = ScalpProvider.Mesh.GetIndices(0).ToList();
            var scalpVertices = ScalpProvider.Mesh.vertices.ToList();

            return ScalpProcessingTools.ProcessIndices(scalpIndices, scalpVertices, HairGroupsProvider.VerticesGroups, GetSegmentsNum(), accuracy)/*.GetRange(144, 9)*/.ToArray();
        }

        public override GpuBuffer<Matrix4x4> GetTransformsBuffer()
        {
            return ScalpProvider.ToWorldMatricesBuffer;
        }

        public override Matrix4x4 GetToWorldMatrix()
        {
            return ScalpProvider.ToWorldMatrix;
        }

        public override int[] GetHairRootToScalpMap()
        {
            return ProcessMap();
        }

        public override Bounds GetBounds()
        {
            return transform.TransformBounds(Bounds);
        }

        public override int GetSegmentsNum()
        {
            return Segments;
        }

        public override int GetStandsNum()
        {
            return HairGroupsProvider.Vertices.Count/Segments;
        }

        public override int[] GetIndices()
        {
            return Indices;
        }

        public override List<Vector3> GetVertices()
        {
            return HairGroupsProvider.Vertices;
        }

        public override List<Color> GetColors()
        {
            return HairGroupsProvider.Colors;
        }

        #region Draw

        private void OnDrawGizmos()
        {
            if(!DebugDraw || GetVertices() == null || !ValidateImpl(false))
                return;

            var scalpToWorld = ScalpProvider.ToWorldMatrix;
            var vertices = GetVertices();

            for (var i = 1; i < vertices.Count; i++)
            {
                if (i % Segments == 0)
                    continue;

                var vertex1 = scalpToWorld.MultiplyPoint3x4(vertices[i - 1]);
                var vertex2 = scalpToWorld.MultiplyPoint3x4(vertices[i]);

                Gizmos.DrawLine(vertex1, vertex2);
            }

            var worldBounds = GetBounds();
            Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
        }

        #endregion
    }
}
