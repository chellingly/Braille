struct VS_OUTPUT
{
	uint id : TEXCOORD0;
};

struct HS_OUTPUT
{
	uint id : TEXCOORD0;
};

struct HS_CONSTANT_OUTPUT
{
	float edges[2] : SV_TessFactor;
};

struct DS_OUTPUT
{
	float4 vertex : SV_POSITION;
	half3 tangent: TANGENT;
	half3 normal : NORMAL;
	half4 factor : TEXCOORD0;
	float3 lightDir: TEXCOORD1;
	half3 viewDir : TEXCOORD2;
	half3 right : TEXCOORD3;
	fixed3 color : TEXCOORD4;
};

struct GS_OUTPUT
{
	float4 pos   : SV_POSITION;
	half3 tangent : TANGENT;
	half3 normal : NORMAL;
	half4 factor : TEXCOORD0;
	float3 lightDir: TEXCOORD1;
	half3 viewDir : TEXCOORD2;
	fixed3 color : TEXCOORD3;
	LIGHTING_COORDS(4,5)
	UNITY_FOG_COORDS(6)	
};

struct GS_OUTPUT_LIGHT
{
	float4 pos   : SV_POSITION;
	half3 tangent : TANGENT;
	half3 normal : NORMAL;
	half4 factor : TEXCOORD0;
	float3 lightDir: TEXCOORD1;
	half3 viewDir : TEXCOORD2;
	float4 lightPos : TEXCOORD3;
	fixed3 color : TEXCOORD4;
	SHADOW_COORDS(5)
};

struct DS_OUTPUT_SHADOW
{
	float4 vertex : SV_POSITION;
	float3 tangent: TANGENT;
};

struct GS_OUTPUT_SHADOW
{
	float4 pos   : SV_POSITION;
	float3 vec : TEXCOORD0;
#ifdef HAIR_MOTION_VECTORS
	half2 motion : TEXCOORD1;
#endif
};

//lighing

half3 ShiftTangent(half3 tangent, half3 normal, half shift)
{
	return normalize(tangent + shift*normal);
}

half Specular(half3 tangent, half3 viewDir, half3 lightDir, half exponent)
{
	half3 h = normalize(viewDir + lightDir);
	half dotTH = dot(tangent, h);
	half sinTH = sqrt(1.0 - dotTH*dotTH);
	half dirAtten = smoothstep(-1.0, 0.0, dotTH);
	return dirAtten * pow(sinTH, exponent);
}

fixed3 Diffuse(half3 normal, half3 lightDir, fixed softness)
{
	fixed dotNL = saturate(dot(normal, lightDir));
	return saturate(lerp(softness, 1, dotNL));
}

fixed3 Fresnel(half3 normal, half3 viewDir, fixed power)
{
	fixed dotNL = 1 - saturate(dot(normal, viewDir));
	return pow(dotNL, power);
}

fixed3 SpecularColor(GS_OUTPUT i, fixed shift, half width1, half width2, fixed3 color)
{
	half3 tangent1 = ShiftTangent(i.tangent, i.normal, i.factor.y - shift);
	half3 tangent2 = ShiftTangent(i.tangent, i.normal, i.factor.y + shift);

	half3 specular1 = Specular(tangent1, i.viewDir, i.lightDir, width1);
	half3 specular2 = Specular(tangent2, i.viewDir, i.lightDir, width2);

	return color*specular1*specular2;
}

fixed3 SpecularColorLight(GS_OUTPUT_LIGHT i, fixed shift, half width1, half width2, fixed3 color)
{
	half3 tangent1 = ShiftTangent(i.tangent, i.normal, i.factor.y - shift);
	half3 tangent2 = ShiftTangent(i.tangent, i.normal, i.factor.y + shift);

	half3 specular1 = Specular(tangent1, i.viewDir, i.lightDir, width1);
	half3 specular2 = Specular(tangent2, i.viewDir, i.lightDir, width2);

	return color*specular1*specular2;
}

//groving
float3 GetBezierPoint(float3 p0, float3 p1, float3 p2, float t)
{
	float invT = 1 - t;
	return invT*invT*p0 + 2 * invT*t*p1 + t*t*p2;
}

uint ToIndex1D(uint x, uint y, uint sizeY)
{
	return x*sizeY + y;
}

struct Body //todo use maping to float3
{
	float3 position;
	float3 lastPosition;
	float radius;
};

struct ParticleData
{
	float3 position;
	float3 velocity;
	float3 tangent;
	fixed3 color;
	fixed interpolation;
};

struct StepData
{
	float3 position;
#ifdef HAIR_MOTION_VECTORS
	float3 velocity;
#endif
	float3 tangent;
	fixed3 color;
};

float3 GetBarycentric(float3 a, float3 b, float3 c, fixed3 k)
{
	return a*k.x + b*k.y + c*k.z;
}

fixed GetBarycentricFixed(fixed a, fixed b, fixed c, fixed3 k)
{
	return a*k.x + b*k.y + c*k.z;
}

half3 Rodrig(half3 e, half3 r, half angle)
{
	half halfA = angle*0.5f;
	half3 a = e*sin(halfA);
	half3 b = cross(a, r);
	half3 q0 = cos(halfA);

	return r + 2*cross(a, b) + 2*q0*b;
}

half Rot(half3 n, half3 v, half a)
{
	return v*cos(a) + cross(n, v)*sin(a);
}

half3 RotateVectorAroundAxis(half3 n, half3 v, half a)
{
	return v*cos(a) + dot(v, n)*n*(1 - cos(a)) + cross(n, v)*sin(a);
}

float CurveVector(half3 n, half3 binormal, half3 uv, half amplitude, half frequency)
{
	half a = uv.x*frequency;
	half r = binormal*amplitude;
	half3 rod = Rodrig(n, r, a);
	return normalize(rod - (dot(n, rod)/dot(rod, rod))*n)*amplitude;
	//return normalize(Rot(n, r, a));
}

half3 CurveDirrection(half3 axis, half3 uv, half amplitude, half frequency)
{
	half angle = uv.x*frequency + uv.z;

	half c = cos(angle);
	half s = sin(angle);

	half3 vecX = half3(0, c, s);
	half3 vecY = half3(c, 0, s);
	half3 vecZ = half3(c, s, 0);

	half3 vec = vecX*axis.x + vecY*axis.y + vecZ*axis.z;

	return vec*amplitude;  
}



