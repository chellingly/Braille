using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
    public class DebugTexture : MonoBehaviour
    {
        public static void SetTexture(Texture texture)
        {
            var obj = new GameObject("DebugTexture");
            var debug = obj.AddComponent<DebugTexture>();
            debug.Texture = texture;
        }

        public Texture Texture { get; set; }

        private void OnGUI()
        {
            GUI.DrawTexture(new Rect(0, 0, 400, 400), Texture, ScaleMode.ScaleToFit, false, 1);
            
        }
    }
}