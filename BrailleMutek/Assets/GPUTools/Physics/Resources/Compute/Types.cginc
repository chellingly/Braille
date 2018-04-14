struct Particle
{
	float3 position;
	float3 lastPosition;
	float radius;
};

struct Sphere
{
	float3 position;
	float radius;
};

struct LineSphere
{
	float3 positionA;
	float3 positionB;
	float radiusA;
	float radiusB;
};

struct PointJoint
{
	int bodyId;
	int matrixId;
	float3 position;
	float elasticity;
};

struct DistanceJoint
{
	int body1Id;
	int body2Id;
	float distance;
	float elasticity;
};



