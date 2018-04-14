using GPUTools.Physics.Scripts.Behaviours;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Tools
{
    public class BoneCollidersPlacer : MonoBehaviour
    {
        [SerializeField] public SkinnedMeshRenderer Skin;
        [SerializeField] public int Depth = 5;

        private Vector3[] vertices;

        [ContextMenu("Process")]
        public void Process()
        {
            Clear();
            Init();
            

            PlaceRecursive(transform, Depth);
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            var lineSpheres = gameObject.GetComponentsInChildren<LineSphereCollider>();
            foreach (var lineSphereCollider in lineSpheres)
                DestroyImmediate(lineSphereCollider);
        }

        //[ContextMenu("Fit")]
        public void Fit()
        {
            Init();
            var lineSpheres = gameObject.GetComponentsInChildren<LineSphereCollider>();
            foreach (var lineSphereCollider in lineSpheres)
            {
                for (int i = 0; i < 20; i++)
                    Rotate(lineSphereCollider, 0.01f);
            }
        }

        [ContextMenu("Grow")]
        public void Grow()
        {
            Init();
            var lineSpheres = gameObject.GetComponentsInChildren<LineSphereCollider>();
            foreach (var lineSphereCollider in lineSpheres)
            {
                lineSphereCollider.RadiusA += 0.01f;
                lineSphereCollider.RadiusB += 0.01f;
            }
        }

        private void Init()
        {
            var mesh = new Mesh();
            Skin.BakeMesh(mesh);
            vertices = mesh.vertices;
        }

        private void PlaceRecursive(Transform bone, int depth)
        {
            depth--;
            if (depth == 0)
                return;

            for (var i = 0; i < bone.childCount; i++)
            {
                var child = bone.GetChild(i);
                AddLineSpheres(bone, child);
                PlaceRecursive(child, depth);
            }
        }

        private void AddLineSpheres(Transform bone, Transform child)
        {
            var lineSphere = bone.gameObject.AddComponent<LineSphereCollider>();
            lineSphere.B = child.localPosition;

            lineSphere.RadiusA = FindNearestMeshDistnce(Skin.transform.InverseTransformPoint(lineSphere.WorldA));
            lineSphere.RadiusB = FindNearestMeshDistnce(Skin.transform.InverseTransformPoint(lineSphere.WorldB));
        }

        private float FindNearestMeshDistnce(Vector3 point)
        {
            var sqrDistance = (vertices[0] - point).sqrMagnitude;

            for (var i = 1; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                var currentSqrDistance = (vertex - point).sqrMagnitude;

                if (currentSqrDistance < sqrDistance)
                {
                    sqrDistance = currentSqrDistance;
                }
            }

            return Mathf.Sqrt(sqrDistance);
        }

        private void Rotate(LineSphereCollider lineSphere, float step)
        {
            for (var i = 0; i < 50; i++)
            {
                var pA = lineSphere.WorldA + RandomVector() * step;
                var rA = FindNearestMeshDistnce(Skin.transform.InverseTransformPoint(pA));

                if (rA > lineSphere.WorldRadiusA)
                {
                    lineSphere.WorldRadiusA = rA;
                    lineSphere.WorldA = pA;
                    break;
                }
            }

            for (var i = 0; i < 50; i++)
            {
                var pB = lineSphere.WorldB + RandomVector() * step;
                var rB = FindNearestMeshDistnce(Skin.transform.InverseTransformPoint(pB));

                if (rB > lineSphere.WorldRadiusB)
                {
                    lineSphere.WorldRadiusA = rB;
                    lineSphere.WorldA = pB;
                    break;
                }
            }
        }

        private Vector3 RandomVector()
        {
            return new Vector3(Random.Range(-1,2), Random.Range(-1, 2), Random.Range(-1, 2));
        }
    }
}
