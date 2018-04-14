using UnityEngine;

namespace GPUTools.HairDemo.Scripts.BallsDemo
{
    public class BallController : MonoBehaviour
    {
        private Rigidbody body;

        private void Start()
        {
            body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.A))
                body.velocity += Vector3.left;

            if(Input.GetKey(KeyCode.D))
                body.velocity += Vector3.right;

            if (Input.GetKey(KeyCode.W))
                body.velocity += Vector3.forward;

            if(Input.GetKey(KeyCode.S))
                body.velocity += Vector3.back;

            body.velocity = Vector3.ClampMagnitude(body.velocity, 2);


        }
    }
}
