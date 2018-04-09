//
// Glitch Fx
//
// Copyright (C) 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class GlitchFx : MonoBehaviour
{
    // Intensity parameter.
    [SerializeField, Range(0, 3)]
    float _intensity = 1.0f;

	[SerializeField, Range(0, 2)]
	float _snowSize = .85f;

    public float intensity {
        get { return _intensity; }
        set { _intensity = value; }
    }

	public float snowSize {
		get { return _snowSize; }
		set { _snowSize = value; }
	}



    // Reference to the shader.
    [SerializeField] Shader shader;

    // Glitch shader and material.
    Material material;

    // Noise texture.
    Texture2D noiseTexture;

    // Old frame buffers.
    RenderTexture oldFrame1;
    RenderTexture oldFrame2;

    // Frame counter for updating buffers.
    int frameCount;

    // Simple random color.
    static Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, Random.value);
    }

    // Initialize the temporary object if it needs.
    void SetUpObjects()
    {
        if (material != null) return;

        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;

		noiseTexture = new Texture2D(64*6, 32*6, TextureFormat.ARGB32, false);
        noiseTexture.hideFlags = HideFlags.DontSave;
        noiseTexture.wrapMode = TextureWrapMode.Clamp;
        noiseTexture.filterMode = FilterMode.Point;

        oldFrame1 = new RenderTexture(Screen.width, Screen.height, 0);
        oldFrame2 = new RenderTexture(Screen.width, Screen.height, 0);

        UpdateNoise();
    }

    // Update the noise texture.
    void UpdateNoise()
    {
        var color = RandomColor();

        for (var x = 0; x < noiseTexture.height; x++)
        {
            for (var y = 0; y < noiseTexture.width; y++)
            {
				if (Random.value > _snowSize) color = RandomColor();
                noiseTexture.SetPixel(y, x, color);
            }
        }

        noiseTexture.Apply();
    }

    void Start()
    {
        SetUpObjects();
    }

    void Update()
    {
        if (Random.value > 0.85f) UpdateNoise();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetUpObjects();

        // Update old frame buffers with the constant interval.
        if ((frameCount % 13) == 0) Graphics.Blit(source, oldFrame1);
        if ((frameCount % 73) == 0) Graphics.Blit(source, oldFrame2);

        // Set up the material.
        material.SetFloat("_Intensity", _intensity);
        material.SetTexture("_GlitchTex", noiseTexture);
        material.SetTexture("_BufferTex", Random.value > 0.5f ? oldFrame1 : oldFrame2);

        // Glitch it!
        Graphics.Blit(source, destination, material);

        frameCount++;
    }
}
