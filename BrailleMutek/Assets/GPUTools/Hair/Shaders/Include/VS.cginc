#ifndef VERTEX
#define VERTEX

VS_OUTPUT VS(uint id:SV_VertexID)
{
	VS_OUTPUT o;
	o.id = id;
	return o;
}

#endif