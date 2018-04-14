using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
    public class BuildDistanceJoints : BuildChainCommand
    {
        private readonly HairSettings settings;

        public BuildDistanceJoints(HairSettings settings)
        {
            this.settings = settings;
        }

        protected override void OnBuild()
        {
            var sizeY = settings.StandsSettings.Segments;

            var distanceJoints = new GroupedData<GPDistanceJoint>();

            var group1 = new List<GPDistanceJoint>();
            var group2 = new List<GPDistanceJoint>();

            for (int i = 0; i < settings.RuntimeData.Particles.Count; i++)
            {
                if (i % sizeY == 0)
                    continue;

                var body1 = settings.RuntimeData.Particles.Data[i - 1];
                var body2 = settings.RuntimeData.Particles.Data[i];
                var distance = Vector3.Distance(body1.Position, body2.Position);//to global

                var list = i % 2 == 0 ? group1 : group2;

                var joint = new GPDistanceJoint(i - 1, i, distance, 0.5f);
                list.Add(joint);
            }

            distanceJoints.AddGroup(group1);
            distanceJoints.AddGroup(group2);

            settings.RuntimeData.DistanceJoints = distanceJoints;
        }

        protected override void OnDispose()
        {
            settings.RuntimeData.DistanceJoints.Dispose();
        }
    }
}
