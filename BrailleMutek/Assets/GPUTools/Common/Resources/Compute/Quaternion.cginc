float4 UnitNormQuaternion(float3 v, float a)
{
	float halfA = a*0.5;

	float sinA = sin(halfA);
	float cosA = cos(halfA);

	return float4(v.x*sinA, v.y*sinA, v.z*sinA, cosA);
}

float4 QuaternionMulQuaternion(float4 q1, float4 q2)
{
	float4 q;
	q.w = q1.w*q2.w - dot(q1.xyz, q2.xyz);
	q.xyz = q1.xyz*q2.w + q2.xyz*q1.w + cross(q1.xyz, q2.xyz);

	return q;
}

float4 QuaternionMulScalar(float4 q, float s)
{
	return q*s;
}

float QuaternionNorm(float4 q)
{
	return sqrt(q.x*q.x + q.y*q.y + q.z*q.z + q.w*q.w);
}

float4 QuaternionNormalize(float4 q)
{
	float invNorm = 1 / QuaternionNorm(q);
	return 	q*invNorm;
}

float4 QuaternionConjugate(float4 q)
{
	q.xyz *= -1;
	return 	q;
}

float4 QuaternionInverse(float4 q)
{
    float norm= QuaternionNorm(q);
    norm = 1/(norm*norm);

    float4 conjugate = QuaternionConjugate(q);

    return conjugate*norm;
}

float3 RotVectorAroundAxis(float3 v, float3 axis, float a)
{
	float4 p = float4(v, 0);//pure quaternion form
	float4 un = UnitNormQuaternion(axis, a);
	float4 unInverse = QuaternionInverse(un);
	float4 rotatedP = QuaternionMulQuaternion(QuaternionMulQuaternion(un, p), unInverse);
	return rotatedP.xyz;
}




