Shader "RDSystem/Surface"
{
    Properties
    {
        _MainTex("RD Texture", 2D) = "white" {}
		_Cutout("CutOut", Range(0, 1)) = 0.2
        [Space]
        _Color0("Color 0", Color) = (1,1,1,1)
        _Color1("Color 1", Color) = (1,1,1,1)
        [Space]
        _Smoothness0("Smoothness 0", Range(0, 1)) = 0.5
        _Smoothness1("Smoothness 1", Range(0, 1)) = 0.5
        [Space]
        _Metallic0("Metallic 0", Range(0, 1)) = 0.0
        _Metallic1("Metallic 1", Range(0, 1)) = 0.0
		[Space]
		_BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}
		_Detail ("Detail", 2D) = "gray" {}
		_HeightMap("Height Map", 2D) = "white" {}
		_HeightMapScale("Height", Float) = 1
		_MetallicGlossMap("Metallic", 2D) = "white" {}
		_OcclusionMap("OcclusionMap", 2D) = "white" {}
		_OcclusionStrength("Occlusion Strength", Float) = 1
        [Space]
        _Threshold("Threshold", Range(0, 1)) = 0.1
        _Fading("Edge Smoothing", Range(0, 1)) = 0.2
        _NormalStrength("Normal Strength", Range(0, 1)) = 0.9
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"  }
		LOD 200
		Cull Off

        CGPROGRAM

        #pragma surface surf Standard alphatest:_Cutout
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _Detail;
		sampler2D _OcclusionMap;
		sampler2D _MetallicGlossMap;
		sampler2D _HeightMap;

        float4 _MainTex_TexelSize;
		float2 __Detail;


        struct Input { float2 uv_MainTex; float2 uv_Detail; float2 uv_BumpMap; };

        fixed4 _Color0, _Color1;
		half _HeightMapScale;
        half _Smoothness0, _Smoothness1;
        half _Metallic0, _Metallic1;
        half _Threshold, _Fading;
		half _OcclusionStrength;
        half _NormalStrength;


		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float4 heightMap = tex2Dlod(_HeightMap, float4(v.texcoord.xy, 0, 0));
			//fixed4 heightMap = _HeightMap;
			v.vertex.z += heightMap.b * _HeightMapScale;
		}


        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float3 duv = float3(_MainTex_TexelSize.xy, 0);
			float4 uv = float4(IN.uv_MainTex, 0, 0);

            half v0 = tex2Dlod(_MainTex, uv).y;
            half v1 = tex2Dlod(_MainTex, uv - duv.xzzz).y;
            half v2 = tex2Dlod(_MainTex, uv + duv.xzzz).y;
            half v3 = tex2Dlod(_MainTex, uv - duv.zyzz).y;
            half v4 = tex2Dlod(_MainTex, uv + duv.zyzz).y;


            half p = smoothstep(_Threshold, _Threshold + _Fading, v0);

			o.Albedo = tex2D(_Detail, IN.uv_Detail).rgb;
			//o.Albedo *= lerp(_Color0.rgb, _Color1.rgb, p);
			//o.Albedo *= tex2D(_Detail, IN.uv_Detail).rgb * 2;
			o.Alpha = v0;
            o.Smoothness = lerp(_Smoothness0, _Smoothness1, p);
			o.Metallic = tex2D(_MetallicGlossMap, IN.uv_Detail) * _Metallic0;
            o.Normal = normalize(float3(v1 - v2, v3 - v4, 1 - _NormalStrength));
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			o.Occlusion = tex2D(_OcclusionMap, IN.uv_Detail) * _OcclusionStrength;

		}

        ENDCG
    }
     Fallback "Transparent/Cutout/Diffuse"
}
