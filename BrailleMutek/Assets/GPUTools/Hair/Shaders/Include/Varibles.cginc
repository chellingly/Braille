#ifndef VARIBLES
#define VARIBLES

half4 _Size;
half4 _TessFactor;
float3 _LightCenter;

uniform StructuredBuffer<ParticleData> _Particles;
uniform StructuredBuffer<fixed3> _Barycentrics;

half _StandWidth;

fixed4 _Length;

fixed _Diffuse;
fixed _FresnelPower;
fixed _FresnelAtten;

fixed _SpecularShift;
half _PrimarySpecular;
half _SecondarySpecular;
fixed4 _SpecularColor;

half3 _WavinessAxis;

sampler2D _ColorTex;

float4x4 _NonJitteredVP;
float4x4 _PreviousVP;
float _MotionVectorDepthBias;

#endif