using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Abstract
{
    public interface IBufferWrapper
    {
        ComputeBuffer ComputeBuffer { get; }
    }
}
