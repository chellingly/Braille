using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Demo
{
    public class DemoCamera : MonoBehaviour
    {
        [SerializeField]private Vector3 lookAt = new Vector3(0, 0.05f, 0);
        private float radius;
        private float angle = Mathf.PI*0.5f;

        private void Awake()
        {
            radius = transform.position.z;
        }

        private void OnEnable()
        {

            //GetComponent<Camera>().depthTextureMode = RenderSetup.DepthNormals;
        }

        private void Update()
        {
            var x = Mathf.Cos(angle)*radius;
            var y = transform.position.y;
            var z = Mathf.Sin(angle)*radius;

            transform.position = new Vector3(x, y, z);
            transform.LookAt(lookAt);

            HandleWheel();
            HandleMove();
        }

        private void HandleWheel()
        {
            radius += Input.GetAxis("Mouse ScrollWheel");
        }

        private void HandleMove()
        {
            if(Input.GetMouseButton(0))
                angle -= Input.GetAxis("Mouse X")*Time.deltaTime;
        }
    }
}
