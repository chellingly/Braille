using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
    public class GPUMatrixMultiplier : KernelBase
    {
        [GpuData("matrices1")]
        public GpuBuffer<Matrix4x4> Matrices1 { get; set; }

        [GpuData("matrices2")]
        public GpuBuffer<Matrix4x4> Matrices2 { get; set; }

        [GpuData("resultMatrices")]
        public GpuBuffer<Matrix4x4> ResultMatrices { get; set; }

        public GPUMatrixMultiplier(GpuBuffer<Matrix4x4> matrices1, GpuBuffer<Matrix4x4> matrices2) : base("Compute/MatrixMultiplier", "CSMatrixMultiplier")
        {
            Matrices1 = matrices1;
            Matrices2 = matrices2;
            
            ResultMatrices = new GpuBuffer<Matrix4x4>(matrices1.Count, sizeof(float)*16);
        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(Matrices1.Count / (float)GpuConfig.NumThreads);
        }

        public override void Dispose()
        {
            ResultMatrices.Dispose();
        }
    }
}
