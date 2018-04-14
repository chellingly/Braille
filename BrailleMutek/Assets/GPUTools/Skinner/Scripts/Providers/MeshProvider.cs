using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Abstract;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Providers
{
    public enum ScalpMeshType
    {
        Static, Skinned
    }

    [Serializable]
    public class MeshProvider : IMeshProvider
    {
        public ScalpMeshType Type = ScalpMeshType.Static;

        [SerializeField] public SkinnedMeshProvider SkinnedProvider = new SkinnedMeshProvider();
        [SerializeField] public StaticMeshProvider StaticProvider = new StaticMeshProvider();

        public bool Validate(bool log)
        {
            return GetCurrentProvider().Validate(log);
        }

        public void Dispatch()
        {
            GetCurrentProvider().Dispatch();
        }

        public void Dispose()
        {
            GetCurrentProvider().Dispose();
        }

        private IMeshProvider GetCurrentProvider()
        {
            if (Type == ScalpMeshType.Static)
                return StaticProvider;

            return SkinnedProvider;
        }

        public Matrix4x4 ToWorldMatrix
        {
            get { return GetCurrentProvider().ToWorldMatrix; }
        }

        public GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
        {
            get { return GetCurrentProvider().ToWorldMatricesBuffer; }
        }

        public Mesh Mesh
        {
            get { return GetCurrentProvider().Mesh; }
        }
    }
}
