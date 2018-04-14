using System;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Settings.Abstract;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Hair.Scripts.Settings
{

    public enum HairHeadCenterType { LocalPoint, Transform }

    /// <summary>
    /// Editor settings for hair geometry 
    /// </summary>
    [Serializable]
    public class HairStandsSettings : HairSettingsBase
    {   
        /// <summary>
        /// Provide geometry gameobject, it should have 2 children
        /// 1) Hair
        /// 2) Scalp
        /// Each child should have mesh filter with mesh on it
        /// </summary>
        public GeometryProviderBase Provider;

        //todo move bounds to provider
        public HairHeadCenterType HeadCenterType = HairHeadCenterType.LocalPoint;

        public Transform HeadCenterTransform;
        public Vector3 HeadCenter;

        public int Segments
        {
            get { return Provider.GetSegmentsNum(); }
        }

        public Vector3 HeadCenterWorld
        {
            get
            {
                if(HeadCenterType == HairHeadCenterType.LocalPoint)
                    return Provider != null 
                        ? Provider.transform.TransformPoint(HeadCenter) 
                        : Vector3.zero;


                return HeadCenterTransform != null
                    ? HeadCenterTransform.position
                    : Vector3.one;
            }
        }

        public override bool Validate()
        {
            Assert.IsNotNull(Provider, "Add Geometry Provider to Hair Settings");
            return Provider != null && Provider.Validate(true);
        }

    }
}
