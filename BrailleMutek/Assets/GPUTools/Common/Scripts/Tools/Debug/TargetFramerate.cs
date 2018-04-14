using UnityEngine;
#pragma warning disable 649

namespace GPUTools.Common.Scripts.Tools.Debug
{
    public class TargetFramerate : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate;

        private void Start()
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
