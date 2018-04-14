Shader "Hidden/AdaptiveEnhanceVR_VIVE"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AEXDer ("Pixel Width", Float) = 1
		_AEYDer ("Pixel Height", Float) = 1 
		_StrSIZE ("Strength", Range(0, 5.0)) = 0.60
		_xMAXAE ("Clamp", Range(0, 1.0)) = 0.05
	}

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half _AEXDer;
				half _AEYDer;
				half _StrSIZE;
				half _StrengthMAX;
				half _xMAXAE;
			
				
				uniform sampler2D _Motion0;
				uniform float _motionDelta;
				uniform float _AdaptiveSharpen;

				fixed4 frag(v2f_img i):COLOR
				{
					half2 coords = i.uv;
					half4 color = tex2D(_MainTex, coords);
					half4 original = color;
					
					float4 mo1 = tex2D(_Motion0, i.uv  );
 					float2 ssVel =  mo1.xy ;
 					//ssVel *=  _motionDelta;
					
					half4 blur  = tex2D(_MainTex, coords + half2(0.5 * _AEXDer,       -_AEYDer));
						  blur += tex2D(_MainTex, coords + half2(      -_AEXDer, 0.5 * -_AEYDer));
						  blur += tex2D(_MainTex, coords + half2(       _AEXDer, 0.5 *  _AEYDer));
						  blur += tex2D(_MainTex, coords + half2(0.5 * -_AEXDer,        _AEYDer));
					blur /= 4;
					
					float delta = lerp(_StrSIZE, _StrengthMAX, saturate(pow(length(ssVel),1.11)*_AdaptiveSharpen) );
					
					half4 lumaStrength = half4(0.2124,0.7153,0.0723, 0) * (delta) * 0.665;

					half4 sharp = color - blur;
					color += clamp(dot(sharp, lumaStrength), -_xMAXAE, _xMAXAE);

					return color; 
				}

			ENDCG
		}

		//=====================================================================

		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half _AEXDer;
				half _AEYDer;
				half _StrSIZE;
				half _xMAXAE;

				fixed4 frag(v2f_img i):COLOR
				{
					half2 coords = i.uv;
					half4 color = tex2D(_MainTex, coords);

					half4 blur  = tex2D(_MainTex, coords + half2(0.5 *  _AEXDer,       -_AEYDer));
						  blur += tex2D(_MainTex, coords + half2(      -_AEXDer, 0.5 * -_AEYDer));
						  blur += tex2D(_MainTex, coords + half2(       _AEXDer, 0.5 *  _AEYDer));
						  blur += tex2D(_MainTex, coords + half2(0.5 * -_AEXDer,        _AEYDer));
					blur /= 4;

					half4 lumaStrength = half4(0.2126, 0.7152, 0.0722, 0) * _StrSIZE * 0.666;
					half4 sharp = color - blur;
					color += clamp(dot(sharp, lumaStrength), -_xMAXAE, _xMAXAE);

					return color;
				}

			ENDCG
		}

		//=====================================================================
	}

	FallBack off
}
