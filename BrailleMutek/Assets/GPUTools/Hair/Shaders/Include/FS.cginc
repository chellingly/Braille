#ifndef FRAGMENT
#define FRAGMENT

#ifdef HAIR_FORWARDBASE
float4 FS(GS_OUTPUT i) :SV_Target
{
    fixed3 lightColor = _LightColor0*LIGHT_ATTENUATION(i)*i.factor.x;

    fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb*i.color;
    fixed3 diffuse = Diffuse(i.normal, i.lightDir, _Diffuse)*i.color*lightColor;
    fixed3 specular = SpecularColor(i, _SpecularShift, _PrimarySpecular, _SecondarySpecular, _SpecularColor)*(max(lightColor, 0.35));

    fixed4 final = fixed4(diffuse + specular + ambient, 1);
    UNITY_APPLY_FOG(i.fogCoord, final);
    return final;
}
#endif

#ifdef HAIR_FORWARDADD
fixed4 FS(GS_OUTPUT_LIGHT i) : SV_Target
{
	fixed cookieAttenuation = 1.0;

#if defined (DIRECTIONAL_COOKIE)
cookieAttenuation = tex2D(_LightTexture0, i.lightPos.xy).a;
#elif defined (POINT_COOKIE)
cookieAttenuation = texCUBE(_LightTexture0, i.lightPos.xyz).a;
#elif defined (SPOT)
cookieAttenuation = tex2D(_LightTexture0, i.lightPos.xy / i.lightPos.w + float2(0.5, 0.5)).a;
#endif

fixed3 lightColor = _LightColor0*cookieAttenuation*SHADOW_ATTENUATION(i);
fixed3 diffuse = Diffuse(i.normal, i.lightDir, _Diffuse)*i.color*lightColor*i.factor.x;
fixed3 specular = SpecularColorLight(i, _SpecularShift, _PrimarySpecular, _SecondarySpecular, _SpecularColor)*lightColor;

return fixed4(diffuse + specular, 1);
}
#endif

#ifdef HAIR_SHADOWCASTER
fixed4 FS(GS_OUTPUT_SHADOW i) : SV_Target
{
	SHADOW_CASTER_FRAGMENT(i)
}
#endif

#ifdef HAIR_NORMAL_DEPTH
fixed4 FS(GS_OUTPUT_SHADOW i) : SV_Target
{
	float depth = i.pos.z;// -(mul(UNITY_MATRIX_V, float4(i.pos, 1)).z * _ProjectionParams.w);
	
	float3 normal = fixed3(0, 0, 1);
	
	return EncodeDepthNormal(depth, normal);
	//return float4(1,1,1,1);
}
#endif

#ifdef HAIR_MOTION_VECTORS
fixed4 FS(GS_OUTPUT_SHADOW i) : SV_Target
{
	return half4(i.motion, 0, 1);
}
#endif

#endif