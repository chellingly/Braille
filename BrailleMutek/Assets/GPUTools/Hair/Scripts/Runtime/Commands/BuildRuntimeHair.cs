using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Runtime.Commands.Physics;
using GPUTools.Hair.Scripts.Runtime.Commands.Render;
using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Runtime.Physics;
using GPUTools.Hair.Scripts.Runtime.Render;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands
{
    public class BuildRuntimeHair : BuildChainCommand
    {
        private readonly HairSettings settings;

        private GameObject obj;
        private GPHairPhysics physics;
        private HairRender render;

        public BuildRuntimeHair(HairSettings settings)
        {
            this.settings = settings;

            Add(new BuildParticles(settings));
            Add(new BuildWind(settings));
            Add(new BuildStaticSpheres(settings));
            Add(new BuildStaticLineSpheres(settings));
            Add(new BuildDistanceJoints(settings));
            Add(new BuildPointJoints(settings));
            Add(new BuildAccessories(settings));

            Add(new BuildBarycentrics(settings));
            Add(new BuildParticlesData(settings));
            Add(new BuildTesselation(settings));
        }

        protected override void OnBuild()
        {
            obj = new GameObject("Render");
            obj.layer = settings.gameObject.layer;
            obj.transform.SetParent(settings.transform.parent, false);

            var data = new HairDataFacade(settings);

            physics = obj.AddComponent<GPHairPhysics>();
            physics.Initialize(data);

            render = obj.AddComponent<HairRender>();
            render.Initialize(data);
        }

        public void FixedDispatch()
        {
            physics.FixedDispatch();
        }
        
        protected override void OnDispatch()
        {
            if(render.IsVisible)
                physics.Dispatch();
            
            render.Dispatch();
        }

        protected override void OnDispose()
        {
            Object.Destroy(obj);
        }

        public bool IsVisible
        {
            get { return render.IsVisible; }
        }
    }
}
