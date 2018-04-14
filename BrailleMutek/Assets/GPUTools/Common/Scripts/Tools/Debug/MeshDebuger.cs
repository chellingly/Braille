using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
    public class MeshDebuger : MonoBehaviour
    {
        [SerializeField] private MeshFilter filter;

        private void Start()
        {
            var vertices = filter.mesh.vertices;

            UnityEngine.Debug.Log("VerticesNum" + vertices.Length);
        }

        private void OnDrawGizmos()
        {
            
        }
    }
}
