using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

namespace GPUTools.Common.Scripts.Tools
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private Text textField;

        float deltaTime;

        private void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

            var ms = deltaTime * 1000.0f;
            var fps = 1.0f / deltaTime;
            textField.text = string.Format("{0:0.0} ms ({1:0.} fps)", ms, fps);
        }
    }
}
