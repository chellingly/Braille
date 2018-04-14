using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
    public class GPUMatrixPointMultiplier : KernelBase
    {
        [GpuData("matrices")]
        public GpuBuffer<Matrix4x4> Matrices { get; set; }

        [GpuData("inPoints")]
        public GpuBuffer<Vector3> InPoints { get; set; }

        [GpuData("outPoints")]
        public GpuBuffer<Vector3> OutPoints { get; set; }

        public GPUMatrixPointMultiplier() : base("Compute/MatrixPointMultiplier", "CSMatrixPointMultiplier")
        {

        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(Matrices.Count / (float)GpuConfig.NumThreads);
        }
    }
}