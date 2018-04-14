Shader "GPUTools/Hair"
{
	Properties
	{
		_ColorTex("Color Tex (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "HairTool" "DisableBatching" = "True" }
		LOD 100

		Pass
		{
			Name "ForwardBase"
			Tags{ "LightMode" = "ForwardBase" }
	
			ZWrite On
		
			CGPROGRAM
			#define HAIR_FORWARDBASE
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "Include/HairTypes.cginc"
			#include "Include/Varibles.cginc"
			#include "Include/VS.cginc"
			#include "Include/Tesselation.cginc"
			#include "Include/GS.cginc"
			#include "Include/FS.cginc"

			#pragma target 5.0
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#pragma vertex VS
			#pragma hull HS
			#pragma domain DS
			#pragma geometry GS
			#pragma fragment FS

			ENDCG
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Forward add pass

		Pass
		{
			Name "ForwardAdd"
			Blend One One

			Tags{ "LightMode" = "ForwardAdd" }

			CGPROGRAM
			#define HAIR_FORWARDADD
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "Include/HairTypes.cginc"
			#include "Include/Varibles.cginc"
			#include "Include/VS.cginc"
			#include "Include/Tesselation.cginc"
			#include "Include/GS.cginc"
			#include "Include/FS.cginc"

			#pragma target 5.0
			#pragma multi_compile_lightpass

			#pragma vertex VS
			#pragma hull HS
			#pragma domain DS
			#pragma geometry GS
			#pragma fragment FS			

			ENDCG
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Shadow pass
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			Fog { Mode Off }

			CGPROGRAM

			#define HAIR_SHADOWCASTER
			#define SHADOW_COORDS(idx1) unityShadowCoord4 _ShadowCoord : TEXCOORD##idx1;
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "Include/HairTypes.cginc"
			#include "Include/Varibles.cginc"
			#include "Include/VS.cginc"
			#include "Include/Tesselation.cginc"
			#include "Include/GS.cginc"
			#include "Include/FS.cginc"

			#pragma target 5.0
			#pragma multi_compile_shadowcaster
			//#pragma multi_compile_fwdbase
			
			#pragma vertex VS
			#pragma hull HS
			#pragma domain DS
			#pragma geometry GS
			#pragma fragment FS

			ENDCG
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Motion Vectors
		Pass 
		{
		    Tags{"RenderType"="Opaque" "LightMode" = "MotionVectors" }
		    Fog { Mode Off }

            ZWrite Off
            
			CGPROGRAM

			#define HAIR_MOTION_VECTORS
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "Include/HairTypes.cginc"
			#include "Include/Varibles.cginc"
			#include "Include/VS.cginc"
			#include "Include/Tesselation.cginc"
			#include "Include/GS.cginc"
			#include "Include/FS.cginc"

			#pragma target 5.0
			#pragma multi_compile_fog
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_fwdbase

			#pragma vertex VS
			#pragma hull HS
			#pragma domain DS
			#pragma geometry GS
			#pragma fragment FS

			ENDCG
		}
	}

	Fallback "VertexLit"
}
