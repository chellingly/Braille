using UnityEngine;

namespace GPUTools.Demo.Common.Scripts
{
	public class PlayImageEffects : MonoBehaviour
	{
		[SerializeField] private Shader shader;

		private Material material;
		
		private void Start () {
			material = new Material(shader);
		}

		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			Graphics.Blit(src, dest, material);
		}
	}
}
