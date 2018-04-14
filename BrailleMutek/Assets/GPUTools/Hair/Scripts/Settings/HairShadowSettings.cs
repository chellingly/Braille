using System;
using GPUTools.Hair.Scripts.Settings.Abstract;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings
{
    [Serializable]
    public class HairShadowSettings : HairSettingsBase
    {
        [SerializeField] public bool CastShadows = true;
        [SerializeField] public bool ReseiveShadows = true;
    }
}
