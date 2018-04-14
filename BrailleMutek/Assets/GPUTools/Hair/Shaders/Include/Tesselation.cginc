#ifndef TESSELATION
#define TESSELATION

HS_CONSTANT_OUTPUT HSConst()
{
	HS_CONSTANT_OUTPUT output;

	output.edges[0] = _TessFactor.x; // Detail factor
	output.edges[1] = _TessFactor.y; // Density factor

	return output;
}

[domain("isoline")]
[partitioning("integer")]
[outputtopology("line")]
[outputcontrolpoints(3)]
[patchconstantfunc("HSConst")]
HS_OUTPUT HS(InputPatch<VS_OUTPUT, 3> ip, uint id : SV_OutputControlPointID)
{
	HS_OUTPUT output;
	output.id = ip[id].id;
	return output;
}

StepData GetPosition(OutputPatch<HS_OUTPUT, 3> op, fixed2 uv)
{
	fixed3 barycentric = _Barycentrics[uv.y*64];
	    
	half index = uv.x*_TessFactor.y;
	
	half length = GetBarycentricFixed(_Length.x, _Length.y, _Length.z, barycentric);
	half length1 = length*index;

	ParticleData p1 = _Particles[op[0].id*_TessFactor.y + length1];
	ParticleData p2 = _Particles[op[1].id*_TessFactor.y + length1];
	ParticleData p3 = _Particles[op[2].id*_TessFactor.y + length1];

	float3 position = GetBarycentric(p1.position, p2.position, p3.position, barycentric);
	position = lerp(position, p1.position, p1.interpolation);

	float3 tangent = GetBarycentric(p1.tangent, p2.tangent, p3.tangent, barycentric);
	tangent = lerp(tangent, p1.tangent, p1.interpolation);	
	
	float3 color = GetBarycentric(p1.color, p2.color, p3.color, barycentric);
	color = lerp(color, p1.color, p1.interpolation);

#ifdef HAIR_MOTION_VECTORS
	float3 velocity = GetBarycentric(p1.velocity, p2.velocity, p3.velocity, barycentric);
	velocity = lerp(velocity, p1.velocity, p1.interpolation);
#endif

	StepData data;
	data.position = position;
#ifdef HAIR_MOTION_VECTORS
	data.velocity = velocity;
#endif
	data.tangent = tangent;
	data.color = color;
	return data;
}

#ifdef HAIR_FORWARDBASE
float4 LightData(float3 position)
{
	return float4(_WorldSpaceLightPos0.xyz, 1);
}

float3 ViewDir(float3 position)
{
	return normalize(_WorldSpaceCameraPos.xyz - position);
}
#endif

#ifdef HAIR_FORWARDADD
float4 LightData(float3 position)
{
	float attenuation = 1;
	float3 lightDirection;
#if defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
	lightDirection = normalize(_WorldSpaceLightPos0.xyz);
#elif defined (POINT_NOATT)
	lightDirection = normalize(_WorldSpaceLightPos0.xyz - position.xyz);
#elif defined(POINT)||defined(POINT_COOKIE)||defined(SPOT)
	float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - position.xyz;
	float distance = length(vertexToLightSource);
	attenuation = 1.0 / (1 + distance);
	lightDirection = normalize(vertexToLightSource);
#endif
	return float4(lightDirection, attenuation);
}

float3 ViewDir(float3 position)
{
	return normalize(_WorldSpaceCameraPos.xyz - position);
}
#endif

#if defined(HAIR_SHADOWCASTER) || defined(HAIR_NORMAL_DEPTH) || defined(HAIR_MOTION_VECTORS)
float4 LightData(float3 position)
{
	return float4(0, 0, 0, 1);
}

float3 ViewDir(float3 position)
{
	return normalize(_WorldSpaceCameraPos.xyz - position);
}
#endif

[domain("isoline")]
DS_OUTPUT DS(HS_CONSTANT_OUTPUT input, OutputPatch<HS_OUTPUT, 3> op, float2 uv : SV_DomainLocation)
{
	DS_OUTPUT output;

	StepData step = GetPosition(op, uv);

	float4 lightData = LightData(step.position);
	float3 lightDir = lightData.xyz;
	half attenuation = lightData.w;

	float3 viewDir = ViewDir(step.position);

	half3 psevdoNormal = normalize(step.position - _LightCenter);
	attenuation *= Diffuse(psevdoNormal, lightDir, _Diffuse) + Fresnel(psevdoNormal, viewDir, _FresnelPower)*_FresnelAtten; 


	half shift = saturate(tex2Dlod(_ColorTex, half4(uv.yx, 0, 0)).r - 0.5);
	fixed thickness = 1 - pow(2, -10 * (1 - uv.x));//curve

	output.vertex = float4(step.position, 1);
	output.tangent = step.tangent;
	output.normal = cross(step.tangent, cross(lightDir, step.tangent));
	output.viewDir = viewDir;
	output.lightDir = lightDir;
	output.factor = half4(saturate(attenuation), shift, 0, 0);
#ifdef HAIR_MOTION_VECTORS
	output.factor = float4(step.velocity, 0);
#endif
	output.right = normalize(cross(step.tangent, output.viewDir))*thickness*_StandWidth;
	output.color = step.color;

	return output;
}

#endif