using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Abstract
{
    public abstract class GeometryProviderBase : MonoBehaviour
    {
        public abstract Bounds GetBounds();
        
        public abstract int GetSegmentsNum();
        public abstract int GetStandsNum();

        public abstract int[] GetIndices();
        public abstract List<Vector3> GetVertices();
        public abstract List<Color> GetColors();

        public abstract GpuBuffer<Matrix4x4> GetTransformsBuffer();
        public abstract Matrix4x4 GetToWorldMatrix();
        public abstract int[] GetHairRootToScalpMap();

        public abstract void Dispatch();
        public abstract bool Validate(bool log);
    }
}
