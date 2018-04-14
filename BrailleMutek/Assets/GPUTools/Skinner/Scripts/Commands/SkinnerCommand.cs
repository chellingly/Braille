using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Common.Scripts.Tools.Kernels;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Commands
{
    public class SkinnerCommand : IBuildCommand
    {
        private readonly SkinnedMeshProvider provider;
        private readonly int[] indices;

        public GpuBuffer<int> Indices { get; set; }
        public GpuBuffer<Matrix4x4> Matrices { get; set; }
        public GpuBuffer<Vector3> LocalPoints { get; set; }
        public GpuBuffer<Vector3> Points { get; set; }
        public GpuBuffer<Matrix4x4> SelectedMatrices { get; set; }
        public GpuBuffer<Vector3> SelectedPoints { get; set; }
        
        private List<KernelBase> kernels = new List<KernelBase>();
        
        public SkinnerCommand(SkinnedMeshProvider provider, int[] indices)
        {
            this.provider = provider;
            this.indices = indices;
        }

        public void Build()
        {
            Matrices = provider.ToWorldMatricesBuffer;
            LocalPoints = new GpuBuffer<Vector3>(provider.Mesh.vertices, sizeof(float)*3);
            Points = new GpuBuffer<Vector3>(provider.Mesh.vertexCount, sizeof(float)*3);
            
            var multiplier = new GPUMatrixPointMultiplier
            {
                InPoints = LocalPoints,
                OutPoints = Points,
                Matrices = Matrices
            };

            kernels.Add(multiplier);

            if (indices != null && indices.Length > 0)
            {
                
                Indices = new GpuBuffer<int>(indices, sizeof(int));
                SelectedPoints = new GpuBuffer<Vector3>(indices.Length, sizeof(float)*3);
                SelectedMatrices = new GpuBuffer<Matrix4x4>(indices.Length, sizeof(float)*16);

                var matrixSelector = new GPUMatrixSelector
                {
                    Indices = Indices,
                    Matrices = Matrices,
                    SelectedMatrices = SelectedMatrices
                };

                var pointsSelector = new GPUPointsSelector
                {
                    Indices = Indices,
                    Points = Points,
                    SelectedPoints = SelectedPoints
                };


                kernels.Add(matrixSelector);
                kernels.Add(pointsSelector);
            }

        }

        public void Dispatch()
        {
            for (var i = 0; i < kernels.Count; i++)
            {
                kernels[i].Dispatch();
            }
        }

        public void UpdateSettings()
        {
            
        }

        public void Dispose()
        {
            if (Indices != null)
                Indices.Dispose();
            
            if (SelectedPoints != null)
                SelectedPoints.Dispose();  
            
            if (SelectedMatrices != null)
                SelectedMatrices.Dispose();
            
            LocalPoints.Dispose();
            Points.Dispose();
        }
    }
}
