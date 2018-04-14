using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
    public class BuildPointJoints:BuildChainCommand
    {
        private readonly HairSettings settings;

        public BuildPointJoints(HairSettings settings)
        {
            this.settings = settings;
        }

        protected override void OnBuild()
        {
            var pointJoints = new GPPointJoint[settings.StandsSettings.Provider.GetVertices().Count];
            CreatePointJoints(pointJoints);

            settings.RuntimeData.PointJoints = new GpuBuffer<GPPointJoint>(pointJoints, GPPointJoint.Size());
        }

        protected override void OnUpdateSettings()
        {
            CreatePointJoints(settings.RuntimeData.PointJoints.Data);
            settings.RuntimeData.PointJoints.PushData();
        }

        private void CreatePointJoints(GPPointJoint[] pointJoints)
        {
            var vertices = settings.StandsSettings.Provider.GetVertices();
            var sizeY = settings.StandsSettings.Segments;
            var map = settings.StandsSettings.Provider.GetHairRootToScalpMap(); //todo resolve long lines problems
            
            for (var i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                var matrixId = map[i / sizeY];

                var t = i % sizeY;

                var elasticity = t == 0
                    ? 1f
                    : Mathf.Clamp01(1 - settings.PhysicsSettings.ElasticityCurve.Evaluate((float)t / sizeY));

                elasticity += JointAreaAdd(vertex);
                pointJoints[i] = new GPPointJoint(i, matrixId, vertex, Mathf.Clamp01(elasticity));
            }
        }

        private float JointAreaAdd(Vector3 vertex)
        {
            var result = 0f;

            foreach (var jointArea in settings.PhysicsSettings.JointAreas)
            {
                var diff = vertex - jointArea.transform.localPosition;

                if (diff.sqrMagnitude < jointArea.Radius * jointArea.Radius)
                    result += 1;
            }

            return result;
        }

        protected override void OnDispose()
        {
            settings.RuntimeData.PointJoints.Dispose();
        }
    }
}
