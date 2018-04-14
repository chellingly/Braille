using GPUTools.Physics.Scripts.Behaviours;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Tools
{
    public class BoneCollidersCopyPaster : MonoBehaviour
    {
        [SerializeField] private Transform from;
        [SerializeField] private Transform to;

        [ContextMenu("CopyPaste")]
        public void CopyPaste()
        {
            CopyPasteRecursive(from, to);   
        }

        private void CopyPasteRecursive(Transform from, Transform to)
        {
            CopyPasteForBone(from, to);

            for (var i = 0; i < from.childCount; i++)
            {
                var fromChild = from.GetChild(i);
                var toChild = to.GetChild(i);

                CopyPasteRecursive(fromChild, toChild);
            }
        }

        private void CopyPasteForBone(Transform from, Transform to)
        {
            var lineSpheres = from.GetComponents<LineSphereCollider>();

            for (var i = 0; i < lineSpheres.Length; i++)
            {
                var lineSphere = lineSpheres[i];

                var newLineSphere = to.gameObject.AddComponent<LineSphereCollider>();

                newLineSphere.WorldA = lineSphere.WorldA;
                newLineSphere.WorldB = lineSphere.WorldB;
                newLineSphere.WorldRadiusA = lineSphere.WorldRadiusA;
                newLineSphere.WorldRadiusB = lineSphere.WorldRadiusB;
            }

            var spheres = from.GetComponents<SphereCollider>();

            for (var i = 0; i < spheres.Length; i++)
            {
                var sphere = spheres[i];

                var newSphere = to.gameObject.AddComponent<SphereCollider>();

                newSphere.center = sphere.center;
                newSphere.radius = sphere.radius;
            }
        }
    }
}
