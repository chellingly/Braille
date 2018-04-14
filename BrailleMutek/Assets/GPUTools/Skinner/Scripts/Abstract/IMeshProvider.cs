using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Abstract
{
    public interface IMeshProvider
    {
        Matrix4x4 ToWorldMatrix { get; }
        GpuBuffer<Matrix4x4> ToWorldMatricesBuffer { get; }
        
        Mesh Mesh { get; } 

        bool Validate(bool log);

        void Dispatch();
        void Dispose();
    }
}
