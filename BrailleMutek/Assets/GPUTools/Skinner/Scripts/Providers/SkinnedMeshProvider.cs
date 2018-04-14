using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Abstract;
using GPUTools.Skinner.Scripts.Kernels;
using GPUTools.Skinner.Scripts.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Skinner.Scripts.Providers
{
    [Serializable]
    public class SkinnedMeshProvider : IMeshProvider
    {
        [SerializeField] public SkinnedMeshRenderer SkinnedMeshRenderer;

        private GPUSkinnerPro gpuSkinner;

        public bool Validate(bool log)
        {
            if(log)
                Assert.IsTrue(SkinnedMeshRenderer != null && SkinnedMeshRenderer.sharedMesh != null, "Skinned mesh rendered field is empty");
            return SkinnedMeshRenderer != null && SkinnedMeshRenderer.sharedMesh != null;
        }

        private void UpdateToWorldMatricesBufferGPU()
        {
            if (gpuSkinner == null)
                gpuSkinner = new GPUSkinnerPro(SkinnedMeshRenderer);
        }

        public GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
        {
            get
            {
                Assert.IsTrue(Application.isPlaying);
                UpdateToWorldMatricesBufferGPU();
                return gpuSkinner.TransformMatricesBuffer;
            }
        }

        public Matrix4x4 ToWorldMatrix
        {
            get { return MeshSkinUtils.CreateToWorldMatrix(SkinnedMeshRenderer); }
        }

        public Mesh Mesh
        {
            get
            {
                return SkinnedMeshRenderer.sharedMesh;
            }
        }

        public void Dispatch()
        {
            if (gpuSkinner != null)
                gpuSkinner.Dispatch();
        }

        public void Dispose()
        {
            if (gpuSkinner != null)
                gpuSkinner.Dispose();
        }
    }
}
