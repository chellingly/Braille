using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Abstract;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Skinner.Scripts.Providers
{
    [Serializable]
    public class StaticMeshProvider : IMeshProvider
    {
        [SerializeField] public MeshFilter MeshFilter;

        private GpuBuffer<Matrix4x4> toWorldMatricesBuffer;
        private GpuBuffer<Matrix4x4> oldToWorldMatricesBuffer;

        public bool Validate(bool log)
        {
            if(log)
                Assert.IsNotNull(MeshFilter, "Mesh Filter field is empty");
            return MeshFilter != null;
        }

        private void UpdateToWorldMatrices()
        {
            if (toWorldMatricesBuffer == null)
            {
                toWorldMatricesBuffer = new GpuBuffer<Matrix4x4>(1, sizeof(float)*16);
            }

            toWorldMatricesBuffer.Data[0] = MeshFilter.transform.localToWorldMatrix;
            toWorldMatricesBuffer.PushData();
        }

        public Matrix4x4 ToWorldMatrix
        {
            get { return MeshFilter.transform.localToWorldMatrix; }
        }

        public GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
        {
            get
            {
                Assert.IsTrue(Application.isPlaying);
                UpdateToWorldMatrices();
                return toWorldMatricesBuffer;
            }
        }

        public Mesh Mesh
        {
            get
            {
                return MeshFilter.sharedMesh;
            }
        }

        public void Dispatch()
        {
            UpdateToWorldMatrices();
        }

        public void Dispose()
        {
            if(toWorldMatricesBuffer != null)
                toWorldMatricesBuffer.Dispose();
        }
    }
}