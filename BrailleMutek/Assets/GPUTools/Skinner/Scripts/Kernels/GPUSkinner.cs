using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Kernels
{
    public struct Weight
    {
        public int bi0;
        public int bi1;
        public int bi2;
        public int bi3;

        public float w0;
        public float w1;
        public float w2;
        public float w3;
    };

    public class GPUSkinner : KernelBase
    {

        private readonly SkinnedMeshRenderer skin;

        private Transform[] bones;
        private Matrix4x4[] bindposes;

        [GpuData("weights")]
        public GpuBuffer<Weight> WeightsBuffer { get; set; }

        [GpuData("bones")]
        public GpuBuffer<Matrix4x4> BonesBuffer { get; set; }

        [GpuData("transforms")]
        public GpuBuffer<Matrix4x4> TransformMatricesBuffer { get; set; }

        public GPUSkinner(SkinnedMeshRenderer skin) : base("Compute/Skinner", "CSComputeMatrices")
        {
            this.skin = skin;
            var mesh = skin.sharedMesh;
            bones = skin.bones;
            bindposes = mesh.bindposes;

            TransformMatricesBuffer = new GpuBuffer<Matrix4x4>(mesh.vertexCount, sizeof (float)*16);
            BonesBuffer = new GpuBuffer<Matrix4x4>(new Matrix4x4[skin.bones.Length], sizeof (float)*16);
            WeightsBuffer = new GpuBuffer<Weight>(GetWeightsArray(mesh), sizeof (int)*4 + sizeof (float)*4);

        }

        public override void Dispatch()
        {
            CalculateBones();
            base.Dispatch();
        }

        public override void Dispose()
        {
            TransformMatricesBuffer.Dispose();
            BonesBuffer.Dispose();
            WeightsBuffer.Dispose();
        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(skin.sharedMesh.vertexCount/ (float)GpuConfig.NumThreads);
        }

        private void CalculateBones()
        {
            for (var i = 0; i < BonesBuffer.Data.Length; i++)
                BonesBuffer.Data[i] = bones[i].localToWorldMatrix*bindposes[i];

            BonesBuffer.PushData();
        }

        private Weight[] GetWeightsArray(Mesh mesh)
        {
            var weights = new Weight[mesh.boneWeights.Length];
            var boneWeights = mesh.boneWeights;

            for (var i = 0; i < boneWeights.Length; i++)
            {
                var boneWeight = boneWeights[i];
                var weight = new Weight
                {
                    bi0 = boneWeight.boneIndex0,
                    bi1 = boneWeight.boneIndex1,
                    bi2 = boneWeight.boneIndex2,
                    bi3 = boneWeight.boneIndex3,

                    w0 = boneWeight.weight0,
                    w1 = boneWeight.weight1,
                    w2 = boneWeight.weight2,
                    w3 = boneWeight.weight3
                };
                weights[i] = weight;
            }

            return weights;
        }
    }
}
