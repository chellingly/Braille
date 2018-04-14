using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
    public class SkinnedMeshCollider : MonoBehaviour
    {
        [SerializeField] private bool debugDraw;
        [SerializeField] private MeshFilter filter;

        private void OnDrawGizmos()
        {
            if(Vertices == null || !debugDraw)
                return;

            Gizmos.color = Color.red;

            foreach (var vertex in Vertices)
            {
                Gizmos.DrawWireSphere(transform.TransformPoint(vertex), 0.01f);
            }
        }

        public Vector3[] Vertices
        {
            get
            {
                var vertices = new Vector3[filter.sharedMesh.vertexCount];
                var localVertices = filter.sharedMesh.vertices;
                for (var i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = transform.TransformPoint(localVertices[i]);
                }

                return vertices;
            }
        }
    }
}
