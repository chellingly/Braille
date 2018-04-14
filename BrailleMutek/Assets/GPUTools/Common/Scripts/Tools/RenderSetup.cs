using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
    [RequireComponent(typeof(Camera))]
    public class RenderSetup : MonoBehaviour
    {
        [SerializeField] private DepthTextureMode mode;
        [SerializeField] private int targetFrameRate;

        private void Start()
        {
            Application.targetFrameRate = targetFrameRate;
            GetComponent<Camera>().depthTextureMode = mode;
        }
    }
}
