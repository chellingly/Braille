using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
    public class DebugVertices:MonoBehaviour
    {
        public static DebugVertices Draw(List<Vector3> vertices, float radius)
        {
            var obj = new GameObject("DebugVertices");
            var debug = obj.AddComponent<DebugVertices>();
            debug.Vertices = vertices;
            debug.Radius = radius;
            return debug;
        }

        public List<Vector3> Vertices;
        public float Radius;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            foreach (var vertex in Vertices)
            {
                Gizmos.DrawWireSphere(vertex, Radius);
            }
        }
    }
}