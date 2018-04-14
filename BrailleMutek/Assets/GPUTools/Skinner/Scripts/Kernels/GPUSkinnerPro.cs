using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Kernels;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Kernels
{
    public class GPUSkinnerPro : PrimitiveBase
    {
        private GPUSkinner skinner;
        private GPUBlendShapePlayer blendShapePlayer;
        private GPUMatrixMultiplier matrixMultiplier;
    
        public GPUSkinnerPro(SkinnedMeshRenderer skin)
        {
            skinner = new GPUSkinner(skin);
            AddPass(skinner);

            if (skin.sharedMesh.blendShapeCount == 0) 
                return;
        
            blendShapePlayer = new GPUBlendShapePlayer(skin);
            matrixMultiplier = new GPUMatrixMultiplier(blendShapePlayer.TransformMatricesBuffer, skinner.TransformMatricesBuffer);

            AddPass(blendShapePlayer);
            AddPass(matrixMultiplier);
        }

        public GpuBuffer<Matrix4x4> TransformMatricesBuffer
        {
            get
            {
                if (matrixMultiplier != null)
                {
                    return matrixMultiplier.ResultMatrices;
                }
            
                return skinner.TransformMatricesBuffer;
            }
        }
    }
}
