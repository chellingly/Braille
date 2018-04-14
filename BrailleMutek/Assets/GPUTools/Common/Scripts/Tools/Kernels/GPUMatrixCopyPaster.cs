using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
    public class GPUMatrixCopyPaster : KernelBase
    {
        [GpuData("matricesFrom")]
        public GpuBuffer<Matrix4x4> MatricesFrom { get; set; }

        [GpuData("matricesTo")]
        public GpuBuffer<Matrix4x4> MatricesTo { get; set; }

        public GPUMatrixCopyPaster(GpuBuffer<Matrix4x4> matricesFrom, GpuBuffer<Matrix4x4> matricesTo) : base("Compute/MatrixCopyPaster", "CSMatrixCopyPaster")
        {
            MatricesFrom = matricesFrom;
            MatricesTo = matricesTo;
        }

        public override int GetGroupsNumX()
        {
            return Mathf.CeilToInt(MatricesFrom.Count / (float)GpuConfig.NumThreads);
        }
    }
}
