using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
    [Serializable]
    public class CreatorGeometry//todo make it generic tool
    {
        public List<GeometryGroupData> List = new List<GeometryGroupData>(); 
        public int SelectedIndex;

        public GeometryGroupData Selected
        {
            get { return SelectedIndex >= 0 && SelectedIndex < List.Count
                    ? List[SelectedIndex] 
                    : null;
            }
        }

        public bool Validate(bool log)
        {
            if (List.Count == 0)
            {
                if(log)
                    Debug.LogError("No geometry was created");
                return false;
            }

            return true;
        }
    }
}
