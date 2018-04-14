using GPUTools.Skinner.Scripts.Kernels;
using UnityEngine;

namespace GPUTools.HairDemo.Scripts.BlendShapesDemo
{
    public class TestBlendShapes : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer skin;

        private GPUSkinnerPro skinner;
        private Vector3[] vertices;

        private void Start()
        {
            skinner = new GPUSkinnerPro(skin);
            skinner.Dispatch();

            vertices = skin.sharedMesh.vertices;
        }

        private void Update()
        {
            skinner.Dispatch();
        }

        private void OnDestroy()
        {
            skinner.Dispose();
        }

        private void OnDrawGizmos()
        {
            if(!Application.isPlaying)
                return;

            skinner.TransformMatricesBuffer.PullData();
            var matrices = skinner.TransformMatricesBuffer.Data;
            
            for (var i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                var transformedVertex = matrices[i].MultiplyPoint3x4(vertex);

                //Gizmos.color = Color.white;
                //Gizmos.DrawWireSphere(skin.transform.TransformPoint(vertex), 0.001f);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transformedVertex, 0.002f);
            }
        }
    }
}
