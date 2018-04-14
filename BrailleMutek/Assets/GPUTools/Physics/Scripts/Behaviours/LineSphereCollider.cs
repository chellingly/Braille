using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
    public class LineSphereCollider : MonoBehaviour
    {
        [SerializeField] public Vector3 A = Vector3.zero;
        [SerializeField] public Vector3 B = new Vector3(0, -0.2f, 0);
        [SerializeField] public float RadiusA = 0.1f;
        [SerializeField] public float RadiusB = 0.1f;
        
        public Vector3 WorldA
        {
            set { A = transform.InverseTransformPoint(value); }
            get { return transform.TransformPoint(A); }
        }

        public Vector3 WorldB
        {
            set { B = transform.InverseTransformPoint(value); }
            get { return transform.TransformPoint(B); }
        }

        public float WorldRadiusA
        {
            set { RadiusA = value/Scale; }
            get { return RadiusA*Scale; }
        }

        public float WorldRadiusB
        {
            set { RadiusB = value/Scale; }
            get { return RadiusB*Scale; }
        }

        private float Scale
        {
            get { return Mathf.Max(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), transform.lossyScale.z); }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(WorldA, WorldRadiusA);
            Gizmos.DrawWireSphere(WorldB, WorldRadiusB);
            
            var dir = Vector3.Normalize(WorldA - WorldB);
            var up = Vector3.Cross(dir, new Vector3(dir.z, dir.y, -dir.x)).normalized;

            var angle = Mathf.PI/10;
            var cos = Mathf.Cos(angle);
            var sin = Mathf.Sin(angle);
            var q = new Quaternion(cos*dir.x, cos*dir.y, cos*dir.z, sin);

            if (q == Quaternion.identity)
                return;

            var identity = Quaternion.identity;
            
            for (var i = 0; i < 5; i++)
            {
                identity *= q;
                
                var mA = Matrix4x4.TRS(WorldA, identity, Vector3.one*WorldRadiusA);
                var mB = Matrix4x4.TRS(WorldB, identity, Vector3.one*WorldRadiusB);

                var p1 = mA.MultiplyPoint3x4(up);
                var p2 = mB.MultiplyPoint3x4(up);

                Gizmos.DrawLine(p1, p2);
            }
        }
    }
}
