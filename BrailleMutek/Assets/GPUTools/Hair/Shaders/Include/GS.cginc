#ifndef GEOMETRY
#define GEOMETRY

#ifdef HAIR_FORWARDBASE
GS_OUTPUT CopyToFragment(DS_OUTPUT v, float4 position)
{
	float4 objectPosition = mul(unity_WorldToObject, position);
	float4 clipPosition = UnityObjectToClipPos(objectPosition);

	GS_OUTPUT output;

	output.pos = clipPosition;
	output.tangent = v.tangent;
	output.normal = v.normal;
	output.viewDir = v.viewDir;
	output.lightDir = v.lightDir;
	output.factor = v.factor;
	output.color = v.color;

	TRANSFER_VERTEX_TO_FRAGMENT(output);
	UNITY_TRANSFER_FOG(output, output.pos);
	
	return output;
}

// Geometry Shader -----------------------------------------------------
[maxvertexcount(4)]
void GS(line DS_OUTPUT p[2], inout TriangleStream<GS_OUTPUT> triStream)
{
	float4 v[4];
	v[0] = float4(p[0].vertex + p[0].right, 1);
	v[1] = float4(p[1].vertex + p[1].right, 1);
	v[2] = float4(p[0].vertex - p[0].right, 1);
	v[3] = float4(p[1].vertex - p[1].right, 1);

	triStream.Append(CopyToFragment(p[0], v[0]));
	triStream.Append(CopyToFragment(p[1], v[1]));
	triStream.Append(CopyToFragment(p[0], v[2]));
	triStream.Append(CopyToFragment(p[1], v[3]));
}
#endif

#ifdef HAIR_FORWARDADD
GS_OUTPUT_LIGHT CopyToFragment(DS_OUTPUT v, float4 position)
{
	float4 objectPosition = mul(unity_WorldToObject, position);
	float4 clipPosition = UnityObjectToClipPos(objectPosition);

	GS_OUTPUT_LIGHT output;

	output.pos = clipPosition;
	output.tangent = v.tangent;
	output.normal = v.normal;
	output.viewDir = v.viewDir;
	output.lightDir = v.lightDir;
	output.factor = v.factor;
	output.color = v.color;

	v.vertex = objectPosition;
#if defined (POINT_COOKIE) || defined (DIRECTIONAL_COOKIE) || defined (SPOT)
	output.lightPos = mul(unity_WorldToLight, position);
#else
	output.lightPos = float4(0, 0, 0, 0);
#endif
	TRANSFER_SHADOW(output);
	return output;
}

// Geometry Shader -----------------------------------------------------
[maxvertexcount(4)]
void GS(line DS_OUTPUT p[2], inout TriangleStream<GS_OUTPUT_LIGHT> triStream)
{
	float4 v[4];
	v[0] = float4(p[0].vertex + p[0].right, 1);
	v[1] = float4(p[1].vertex + p[1].right, 1);
	v[2] = float4(p[0].vertex - p[0].right, 1);
	v[3] = float4(p[1].vertex - p[1].right, 1);

	triStream.Append(CopyToFragment(p[0], v[0]));
	triStream.Append(CopyToFragment(p[1], v[1]));
	triStream.Append(CopyToFragment(p[0], v[2]));
	triStream.Append(CopyToFragment(p[1], v[3]));
}
#endif

#if defined(HAIR_SHADOWCASTER) || defined(HAIR_NORMAL_DEPTH) || defined(HAIR_MOTION_VECTORS) 
GS_OUTPUT_SHADOW CopyToFragment(DS_OUTPUT v, float4 position)
{
	v.vertex = mul(unity_WorldToObject, position);

	GS_OUTPUT_SHADOW output;

    output.vec = position.xyz - _LightPositionRange.xyz;
	output.pos = UnityObjectToClipPos(v.vertex.xyz);

#ifdef HAIR_MOTION_VECTORS
	float4 prevClipPos = mul(_PreviousVP, position - v.factor);//factor is velocity for motion vectors flag
	float4 curClipPos = mul(_NonJitteredVP, position);

    float2 prevHPos = prevClipPos.xy / prevClipPos.w;
    float2 curHPos = curClipPos.xy / curClipPos.w;

    float2 vPosPrev = (prevHPos.xy + 1.0f) / 2.0f;
    float2 vPosCur = (curHPos.xy + 1.0f) / 2.0f;

#if UNITY_UV_STARTS_AT_TOP
    vPosPrev.y = 1.0 - vPosPrev.y;
    vPosCur.y = 1.0 - vPosCur.y;
#endif

	#if defined(UNITY_REVERSED_Z)
            output.pos.z -= _MotionVectorDepthBias * output.pos.w;
	#else
            output.pos.z += _MotionVectorDepthBias * output.pos.w;
	#endif

	output.motion = vPosCur - vPosPrev;
#endif

	return output;
}

// Geometry Shader -----------------------------------------------------
[maxvertexcount(4)]
void GS(line DS_OUTPUT p[2], inout TriangleStream<GS_OUTPUT_SHADOW> triStream)
{
	float4 v[4];
	v[0] = float4(p[0].vertex + p[0].right, 1);
	v[1] = float4(p[1].vertex + p[1].right, 1);
	v[2] = float4(p[0].vertex - p[0].right, 1);
	v[3] = float4(p[1].vertex - p[1].right, 1);

	triStream.Append(CopyToFragment(p[0], v[0]));
	triStream.Append(CopyToFragment(p[1], v[1]));
	triStream.Append(CopyToFragment(p[0], v[2]));
	triStream.Append(CopyToFragment(p[1], v[3]));
}
#endif

#endif