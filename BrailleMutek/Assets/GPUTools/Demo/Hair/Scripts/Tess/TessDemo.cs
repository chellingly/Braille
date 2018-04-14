using System.Collections.Generic;
using GPUTools.Common.Scripts.Tools;
using UnityEngine;

namespace GPUTools.HairDemo.Scripts.Tess
{
    public class TessDemo : MonoBehaviour
    {
        [SerializeField][Range(6, 64)] private int count = 64;
        private int oldCount = 0;

        [SerializeField] private Vector3 a;
        [SerializeField] private Vector3 b;
        [SerializeField] private Vector3 c;

        private FixedList<Vector3> barycentric = new FixedList<Vector3>(64);

        private void Start()
        {
            Gen();
            oldCount = count;
        }

        private void Update()
        {
            
            if(count != oldCount)
                Gen();

            oldCount = count;
        }
        private void Gen()
        {
            barycentric.Reset();

            var n = 1;
            var off = 0.2f;
            if (count >= 15)
            {
                off = 0.1f;
                n = 2;
            }
            if (count >= 45)
            {
                off = 0.05f;
                n = 3;
            }

            var m = 1 - off;
            var mm = (1 - m) * 0.5f;
            Split(new Vector3(m, mm, mm), new Vector3(mm, m, mm), new Vector3(mm, mm, m), n);

            while (barycentric.Count < count)
            {
                var k = GetRandomK();
                if (!barycentric.Contains(k))
                    barycentric.Add(GetRandomK());
            }

            Debug.Log(barycentric.Count);
        }

        private void Split(Vector3 b1, Vector3 b2, Vector3 b3, int steps)
        {
            steps--;
            
            TryAdd(b1);
            TryAdd(b2);
            TryAdd(b3);

            var n1 = (b1 + b2) * 0.5f;
            var n2 = (b2 + b3) * 0.5f;
            var n3 = (b3 + b1) * 0.5f;

            if(steps < 0)
                return;

            Split(b1, n1, n3, steps);
            Split(b2, n1, n2, steps);
            Split(b3, n2, n3, steps);
            Split(n1, n2, n3, steps);
        }

        private void TryAdd(Vector3 v)
        {
            if(!barycentric.Contains(v))
                barycentric.Add(v);
        }

       

        private Vector3 GetRandomK()
        {
            var ka = Random.Range(0f, 1f);
            var kb = Random.Range(0f, 1f);

            if (ka + kb > 1)
            {
                ka = 1 - ka;
                kb = 1 - kb;
            }

            var kc = 1 - (ka + kb);
            return new Vector3(ka, kb, kc);
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
            Gizmos.DrawLine(c, a);

            Gizmos.color = new Color(1,0,0,1);

            for (var i = 0; i < barycentric.Count; i++)
            {
                var bar = barycentric[i];
                var p = a * bar.x + b * bar.y + c * bar.z;
                Gizmos.DrawSphere(p, 0.01f);
            }
        }
    }
}
