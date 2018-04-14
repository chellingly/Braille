using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Kernels
{
    public class GPUBlendShapePlayer : KernelBase
    {
        private readonly SkinnedMeshRenderer skin;
        private readonly Mesh mesh;

        [GpuData("vertexCount")]
        public GpuValue<int> VertexCount { get; set; }
        
        [GpuData("shapesCount")]
        public GpuValue<int> ShapesCount { get; set; }
        
        [GpuData("localToWorld")]
        public GpuValue<GpuMatrix4x4> LocalToWorld { get; set; }
        
        [GpuData("shapes")]
        public GpuBuffer<Vector3> ShapesBuffer { get; set; }
        
        [GpuData("weights")]
        public GpuBuffer<float> WeightsBuffer { get; set; }
        
        [GpuData("transforms")]
        public GpuBuffer<Matrix4x4> TransformMatricesBuffer { get; set; }
        
        public GPUBlendShapePlayer(SkinnedMeshRenderer skin) : base("Compute/BlendShaper", "CSBlendShaper")
        {
            this.skin = skin;
            mesh = skin.sharedMesh;
            
            VertexCount = new GpuValue<int>(mesh.vertexCount);
            ShapesCount = new GpuValue<int>(mesh.blendShapeCount);
            LocalToWorld = new GpuValue<GpuMatrix4x4>(new GpuMatrix4x4(skin.localToWorldMatrix));

            ShapesBuffer = new GpuBuffer<Vector3>(GetAllShapes(), sizeof(float)*3);
            WeightsBuffer = new GpuBuffer<float>(mesh.blendShapeCount, sizeof(float));
            TransformMatricesBuffer = new GpuBuffer<Matrix4x4>(mesh.vertexCount, sizeof (float)*16);
        }

        private Vector3[] GetAllShapes()
        {
            var result = new List<Vector3>();
            var deltaVertices = new Vector3[VertexCount.Value];
            for (var i = 0; i < mesh.blendShapeCount; i++)
            {
                mesh.GetBlendShapeFrameVertices(i, 0, deltaVertices, null, null);
                result.AddRange(deltaVertices);
            }

            return result.ToArray();
        }
        
        public override void Dispatch()
        {
            LocalToWorld.Value.Data = skin.localToWorldMatrix;
            PushWeights();
            base.Dispatch();
        }

        private void PushWeights()
        {
            for (var i = 0; i < mesh.blendShapeCount; i++)
            {
                var weight = Mathf.Clamp01(skin.GetBlendShapeWeight(i) * 0.01f);
                WeightsBuffer.Data[i] = weight;
            }

            WeightsBuffer.PushData();
        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(VertexCount.Value/ (float)GpuConfig.NumThreads);
        }

        public override void Dispose()
        {
            ShapesBuffer.Dispose();
            WeightsBuffer.Dispose();
            TransformMatricesBuffer.Dispose();
        }
    }
}