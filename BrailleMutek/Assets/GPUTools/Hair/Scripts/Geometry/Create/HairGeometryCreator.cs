using System;
using System.Collections.Generic;
using System.Linq;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.Tools;
using GPUTools.Hair.Scripts.Utils;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
    public enum ScalpRendererType { Mesh, SkinnedMesh}

    [Serializable]
    public class HairGeometryCreator : GeometryProviderBase
    {
        [SerializeField]public bool DebugDraw = false;
        [SerializeField]public int Segments = 5;
        [SerializeField]public GeometryBrush Brush = new GeometryBrush();
        [SerializeField]public MeshProvider ScalpProvider = new MeshProvider();
        [SerializeField]public List<GameObject> ColliderProviders = new List<GameObject>();
        [SerializeField]public CreatorGeometry Geomery = new CreatorGeometry();
        [SerializeField]public Bounds Bounds;
        
        [SerializeField]private int[] indices;
        [SerializeField]private List<Vector3> vertices;
        [SerializeField]private List<Color> colors;
        [SerializeField]private int[] hairRootToScalpIndices;

        [SerializeField] private bool isProcessed = false;
        private void Awake()
        {
            if(!isProcessed)
                Process();
        }

        public void Optimize()
        {
            Process();
        }

        public void SetDirty()
        {
            isProcessed = false;
        }
        
        public void Process()
        {
            if (!ScalpProvider.Validate(true))
                return;
            
            var listVerticesGroup = new List<List<Vector3>>();
            var verticesList = new List<Vector3>();
            var colorsList = new List<Color>();

            foreach (var groupData in Geomery.List)
            {
                listVerticesGroup.Add(groupData.Vertices);
                verticesList.AddRange(groupData.Vertices);
                colorsList.AddRange(groupData.Colors);
            }

            vertices = verticesList;
            colors = colorsList;

            var scalpMesh = ScalpProvider.Mesh;
            var accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(scalpMesh)*0.1f;
            indices = ScalpProcessingTools.ProcessIndices(scalpMesh.GetIndices(0).ToList(), scalpMesh.vertices.ToList(), listVerticesGroup, Segments, accuracy).ToArray();
            
            //TODO TO ADD SUBMESHES SUPPORT AND SUB HAIRS AS INDEPENDENT GAME OBJECTS
            
            if (ScalpProvider.Type == ScalpMeshType.Skinned)
            {
                hairRootToScalpIndices = ScalpProcessingTools.HairRootToScalpIndices(scalpMesh.vertices.ToList(), vertices, Segments, accuracy).ToArray();
            }
            else
            {
                hairRootToScalpIndices = new int[vertices.Count / GetSegmentsNum()];
            }

            isProcessed = true;
        }

        public override void Dispatch()
        {
            ScalpProvider.Dispatch();
        }

        public override bool Validate(bool log)
        {
            return ScalpProvider.Validate(log) && Geomery.Validate(log);
        }

        private void OnDestroy()
        {
            ScalpProvider.Dispose();
        }

        public override Bounds GetBounds()
        {
            return  transform.TransformBounds(Bounds);
        }

        public override int GetSegmentsNum()
        {
            return Segments;
        }
        public override int GetStandsNum()
        {
            return vertices.Count / Segments;
        }

        public override int[] GetIndices()
        {
            return indices;
        }

        public override List<Vector3> GetVertices()
        {
            return vertices;
        }

        public override List<Color> GetColors()
        {
            return colors;
        }

        public override Matrix4x4 GetToWorldMatrix()
        {
            return ScalpProvider.ToWorldMatrix;
        }

        public override GpuBuffer<Matrix4x4> GetTransformsBuffer()
        {
            return ScalpProvider.ToWorldMatricesBuffer;
        }

        public override int[] GetHairRootToScalpMap()
        {
            return hairRootToScalpIndices;
        }

        #region DebugDraw

        private void OnDrawGizmos()
        {
            if(!DebugDraw || !ScalpProvider.Validate(false) )
                return;
            
            foreach (var data in Geomery.List)
            {
                var isSelected = Geomery.Selected == data;

                data.OnDrawGizmos(Segments, isSelected, ScalpProvider.ToWorldMatrix);
            }

            var worldBounds = GetBounds();
            Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
        }

        #endregion
    }
}
