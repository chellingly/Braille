using UnityEngine;

namespace GPUTools.HairDemo.Scripts
{
    public class ConstantRotation:MonoBehaviour
    {
        [SerializeField] private Vector3 axis;
        [SerializeField] public float Speed;

        private void Update()
        {
            transform.Rotate(axis, Speed*Time.deltaTime);
        }
    }
}