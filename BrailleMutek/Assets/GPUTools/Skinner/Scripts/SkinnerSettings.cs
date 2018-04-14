using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Commands;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Skinner.Scripts
{
    public class SkinnerSettings : MonoBehaviour
    {
        [SerializeField] public bool DebugDraw;
        [SerializeField] public SkinnedMeshProvider MeshProvider = new SkinnedMeshProvider();

        private SkinnerCommand command;

        public void Initialize(int[] indices = null)
        {
            command = new SkinnerCommand(MeshProvider, indices);
            command.Build();
        }

        private void OnDestroy()
        {
            MeshProvider.Dispose();
            
            if(command != null)
                command.Dispose();
        }

        public void Dispatch()
        {
            if(!MeshProvider.Validate(false) || command == null)
                return;
            
            MeshProvider.Dispatch();
            command.Dispatch();
        }

        public GpuBuffer<Matrix4x4> SelectedToWorldMatricesBuffer
        {
            get { return command.SelectedMatrices; }
        }

        public GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
        {
            get { return MeshProvider.ToWorldMatricesBuffer; }
        }
        
        public GpuBuffer<Vector3> SelectedWorldVerticesBuffer
        {
            get { return command.SelectedPoints; }
        }
        
        public GpuBuffer<Vector3> WorldVerticesBuffer
        {
            get { return command.Points; }
        }

        #region
        private void OnDrawGizmos()
        {
            if(!DebugDraw || !Application.isPlaying || !MeshProvider.Validate(false))
                return;

            var triangles = MeshProvider.Mesh.triangles;
            var vertices = MeshProvider.Mesh.vertices;
            
            MeshProvider.Dispatch();
            MeshProvider.ToWorldMatricesBuffer.PullData();

            var matrices = MeshProvider.ToWorldMatricesBuffer.Data;

            Gizmos.color = Color.magenta;

            for (var i = 0; i < triangles.Length; i += 3)
            {
                var i0 = triangles[i];
                var i1 = triangles[i + 1];
                var i2 = triangles[i + 2];

                var v0 = matrices[i0].MultiplyPoint3x4(vertices[i0]);
                var v1 = matrices[i1].MultiplyPoint3x4(vertices[i1]);
                var v2 = matrices[i2].MultiplyPoint3x4(vertices[i2]);

                Gizmos.DrawLine(v0, v1);
                Gizmos.DrawLine(v1, v2);
                Gizmos.DrawLine(v2, v0);
            }
        }
        #endregion
    }
}
