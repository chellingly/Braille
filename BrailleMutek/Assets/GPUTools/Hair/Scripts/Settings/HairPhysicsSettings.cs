using System;
using System.Collections.Generic;
using System.Linq;
using GPUTools.Hair.Scripts.Geometry.Constrains;
using GPUTools.Hair.Scripts.Settings.Abstract;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Hair.Scripts.Settings
{
    /// <summary>
    /// Physics simulation settings 
    /// </summary>
    [Serializable]
    public class HairPhysicsSettings : HairSettingsBase
    {
        public bool DebugDraw = false;

        public bool IsEnabled = true;
        
        //quality
        public int Iterations = 1;
        public bool FastMovement = true;

        //stands
        public Vector3 Gravity = new Vector3(0,-1, 0);
        public float Drag = 0;
        public float StandRadius = 0.1f;

        //stands elasticy
        public AnimationCurve ElasticityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public float WindMultiplier = 0.0001f;

        //colliders
        public List<GameObject> ColliderProviders = new List<GameObject>();

        //accessories
        public List<GameObject> AccessoriesProviders = new List<GameObject>();

        //Joints
        public List<HairJointArea> JointAreas = new List<HairJointArea>();

        public bool UseDeltaTime = false;

        #region compute data

        private List<SphereCollider> colliders;


        public List<SphereCollider> GetColliders()
        {
            var list = new List<SphereCollider>();

            foreach (var provider in ColliderProviders)
                list.AddRange(provider.GetComponents<SphereCollider>().ToList());

            return list;
        }

        #endregion

        public override bool Validate()
        {
            foreach (var colliderProvider in ColliderProviders)
            {
                if (colliderProvider == null)
                {
                    Debug.LogError("Setup Colliders Provider in Physics Settings it can't be null.");
                    return false;
                }
            }

            return true;
        }
    }
}
