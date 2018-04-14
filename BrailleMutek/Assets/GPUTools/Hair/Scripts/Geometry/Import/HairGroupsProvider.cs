using System;
using System.Collections.Generic;
using System.Linq;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Common.Scripts.Tools.Debug;
using GPUTools.Hair.Scripts.Geometry.Tools;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Import
{
    [Serializable]
    public class HairGroupsProvider
    {
        [SerializeField] public List<MeshFilter> HairFilters = new List<MeshFilter>();

        [SerializeField] public List<List<Vector3>> VerticesGroups;
        [SerializeField] public List<Vector3> Vertices;

        [SerializeField] public List<List<Color>> ColorsGroups;
        [SerializeField] public List<Color> Colors;

        public void Process(Matrix4x4 worldToObject)
        {
            VerticesGroups = InitVerticesGroups(worldToObject);
            Vertices = InitVertices();
            ColorsGroups = InitColorGroups();
            Colors = InitColors();
        }

        private List<List<Vector3>> InitVerticesGroups(Matrix4x4 worldToObject)
        {
            return HairFilters.Select(filter => MeshUtils.GetVerticesInSpace(filter.sharedMesh, filter.transform.localToWorldMatrix, worldToObject)).ToList();
        }

        private List<Vector3> InitVertices()
        { 
            return VerticesGroups.SelectMany(verticesGroup => verticesGroup).ToList();
        }

        private List<List<Color>> InitColorGroups()
        {
            return HairFilters.Select(filter => filter.sharedMesh.colors.ToList()).ToList();
        }

        private List<Color> InitColors()
        { 
            return ColorsGroups.SelectMany(colorsGroup => colorsGroup).ToList();
        }

        public bool Validate(bool log)
        {
            if (Validator.TestList(HairFilters))
                return true;

            if(log)
                Debug.LogError("Hair list is empty or contains empty elements ");
            
            return false;
        }
    }
}
