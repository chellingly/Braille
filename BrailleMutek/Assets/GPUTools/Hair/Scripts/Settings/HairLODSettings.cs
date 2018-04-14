using System;
using GPUTools.Common.Scripts.Tools.Ranges;
using GPUTools.Hair.Scripts.Settings.Abstract;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings
{
    /// <summary>
    /// Level of detail settings
    /// </summary>
    [Serializable]
    public class HairLODSettings : HairSettingsBase
    {
        public Camera ViewCamera;
        
        public FloatRange Distance = new FloatRange(0, 5);
        public FloatRange Density = new FloatRange(4, 8);
        public FloatRange Detail = new FloatRange(4, 16);
        public FloatRange Width = new FloatRange(0.0004f, 0.002f);


        public float GetWidth(Vector3 position)
        {
            return Width.GetLerp(GetDistanceK(position));
        }

        public int GetDencity(Vector3 position)
        {
            return (int)Density.GetLerp(1 - GetDistanceK(position));
        }

        public int GetDetail(Vector3 position)
        {
            return (int)Detail.GetLerp(1 - GetDistanceK(position));
        }

        public float GetDistanceK(Vector3 position)
        {
            var k = (GetDistanceToCamera(position) - Distance.Min) /(Distance.Max - Distance.Min);

            return Mathf.Clamp01(k);
        }

        public float GetDistanceToCamera(Vector3 position)
        {
            if (ViewCamera != null)
                return (position - ViewCamera.transform.position).magnitude;
            
            return (position - Camera.main.transform.position).magnitude;
        }

        public bool IsPhysicsEnabled(Vector3 position)
        {
            return GetDistanceToCamera(position) < Distance.Max;
        }
    }
}
