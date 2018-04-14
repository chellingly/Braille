using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Config;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
    public class DistanceJointsKernel : KernelBase
    {
        [GpuData("step")] public GpuValue<float> Step { set; get; }
        [GpuData("particles")] public GpuBuffer<GPParticle> Particles { set; get; }
        [GpuData("distanceJoints")] public GpuBuffer<GPDistanceJoint> DistanceJoints { set; get; }

        public GroupedData<GPDistanceJoint> GroupedData { set; get; }

        public DistanceJointsKernel(GroupedData<GPDistanceJoint> groupedData) : base("Compute/DistanceJoints", "CSDistanceJoints")
        {
            GroupedData = groupedData;
            DistanceJoints = new GpuBuffer<GPDistanceJoint>(groupedData.Data, GPDistanceJoint.Size());
        }

        public override void Dispatch()
        {
            if (!IsEnabled)
                return;

            if (Props.Count == 0)
                CacheAttributes();

            BindAttributes();

            for (var i = 0; i < GroupedData.GroupsData.Count; i++)
            {
                var groupData = GroupedData.GroupsData[i];
                
                Shader.SetInt("startGroup", groupData.Start);
                Shader.SetInt("sizeGroup", groupData.Num);
                
                var threatsNum = Mathf.CeilToInt(groupData.Num / (float)GpuConfig.NumThreads);
                Shader.Dispatch(KernelId, threatsNum, 1, 1);
            }
        }

        public override void Dispose()
        {
            DistanceJoints.Dispose();
        }
    }
}
