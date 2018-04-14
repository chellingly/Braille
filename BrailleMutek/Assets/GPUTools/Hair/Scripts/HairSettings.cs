using GPUTools.Common.Scripts.Tools;
using GPUTools.Common.Scripts.Tools.Debug;
using GPUTools.Hair.Scripts.Runtime.Commands;
using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Settings;
using UnityEngine;

namespace GPUTools.Hair.Scripts
{
    /// <summary>
    /// This class is access point to all hair settings. 
    /// Hair settings consist of one or multiple hair group settings
    /// On start it creates game object (to render hair) for each group settings
    /// </summary>
    public class HairSettings : MonoBehaviour
    {
        public HairStandsSettings StandsSettings = new HairStandsSettings();
        public HairPhysicsSettings PhysicsSettings = new HairPhysicsSettings();
        public HairRenderSettings RenderSettings = new HairRenderSettings();
        public HairLODSettings LODSettings = new HairLODSettings();
        public HairShadowSettings ShadowSettings = new HairShadowSettings();

        public RuntimeData RuntimeData { private set; get; }
        public BuildRuntimeHair HairBuidCommand { private set; get; }

        public FloatSmoother DeltaTime { private set; get; }

        private void Start()
        {  
            if(!ValidateImpl())
                return;

            DeltaTime = new FloatSmoother(20);
            RuntimeData = new RuntimeData();
            HairBuidCommand = new BuildRuntimeHair(this);
            HairBuidCommand.Build();
        }

        public void ReStart()
        {
            if (!ValidateImpl())
                return;

            if (HairBuidCommand != null)
                HairBuidCommand.Dispose();

            Start();
        }

        private void FixedUpdate()
        {
            if(HairBuidCommand == null)
                return;
            
            HairBuidCommand.FixedDispatch();
        }

        private void LateUpdate()
        {
            if(HairBuidCommand == null)
                return;

            DeltaTime.AddValue(Time.deltaTime);
            StandsSettings.Provider.Dispatch();
            HairBuidCommand.Dispatch();
        }

        public void UpdateSettings()
        {
            if (HairBuidCommand == null)
                return;

            if (Application.isPlaying)
                HairBuidCommand.UpdateSettings();
        }

        public void OnDestroy()
        {
            if(HairBuidCommand != null)
                HairBuidCommand.Dispose();
        }

        private bool ValidateImpl()
        {
            return StandsSettings.Validate() 
                && PhysicsSettings.Validate()
                && RenderSettings.Validate()
                && LODSettings.Validate()
                && ShadowSettings.Validate();
        }

        private void OnDrawGizmos()
        {
            StandsSettings.DrawGizmos();
            PhysicsSettings.DrawGizmos();
            RenderSettings.DrawGizmos();
            LODSettings.DrawGizmos();
            ShadowSettings.DrawGizmos();
        }
    }
}
