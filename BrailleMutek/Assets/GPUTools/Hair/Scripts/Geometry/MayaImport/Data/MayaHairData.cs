using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Data
{
    [Serializable]
    public class MayaHairData
    {
        [SerializeField] public int Segments;

        [SerializeField] public List<Vector3> Lines;
        [SerializeField] public List<Vector3> TringlesCenters;

        [SerializeField] public int[] HairRootToScalpMap;

        [SerializeField] public int[] Indices;
        [SerializeField] public List<Vector3> Vertices;

        public bool Validate(bool log)
        {
            if (Indices == null || Indices.Length == 0)
            {
                if(log)
                    UnityEngine.Debug.LogError("Maya data was not generated succesfuly");
                return false;
            }

            return true;
        }
    }
}
