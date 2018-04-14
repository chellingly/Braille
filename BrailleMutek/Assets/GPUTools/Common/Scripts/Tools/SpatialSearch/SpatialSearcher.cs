using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.SpatialSearch
{
    public class SpatialSearcher
    {
        private readonly List<SearchVoxel> voxels;
        private readonly List<Vector3> vertices;
        private readonly Bounds bounds;

        private FixedList<int> fixedList;

        public SpatialSearcher(List<Vector3> vertices, Bounds bounds, int splitX, int splitY, int splitZ)
        {
            this.vertices = vertices;
            this.bounds = bounds;

            voxels = CreateVoxels(splitX, splitY, splitZ);
            fixedList = new FixedList<int>(vertices.Count);
        }

        private List<SearchVoxel> CreateVoxels(int splitX, int splitY, int splitZ)
        {
            var subBoundsSize = new Vector3(bounds.size.x / splitX, bounds.size.y / splitY, bounds.size.z / splitZ);
            var result = new List<SearchVoxel>();

            for (var x = 0; x <= splitX; x++)
            {
                for (var y = 0; y <= splitY; y++)
                {
                    for (var z = 0; z <= splitZ; z++)
                    {
                        var subBoundsCenter = bounds.center + new Vector3(subBoundsSize.x*x, subBoundsSize.y*y, subBoundsSize.z*z) - bounds.size*0.5f;
                        var subBounds = new Bounds(subBoundsCenter, subBoundsSize);
                        var voxel = new SearchVoxel(vertices, subBounds);

                        if(voxel.TotalVertices > 0)
                            result.Add(voxel);
                    }
                }
            }

            return result;
        }

        public FixedList<int> SearchInSphereSlow(Vector3 center, float radius)
        {
            fixedList.Reset();

            for (var i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                if ((center - vertex).sqrMagnitude < radius*radius) 
                    fixedList.Add(i);
            }

            return fixedList;
        }        
        
        public List<int> SearchInSphereSlow(Ray ray, float radius)
        {
            var result = new List<int>();
            var sqrRadius = radius * radius;
            for (var i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                var sqrDistance = Vector3.Cross(ray.direction, vertex - ray.origin).sqrMagnitude;
                if (sqrDistance < sqrRadius)
                    result.Add(i);
            }

            return result;
        }

        public List<int> SearchInSphere(Vector3 center, float radius)
        {
            var voxels = SearchVoxelsInSphere(center, radius);
            var result = new List<int>();

            foreach (var voxel in voxels)
                result.AddRange(voxel.SearchInSphere(center, radius));

            return result;
        }        
        
        public List<int> SearchInSphere(Ray ray, float radius)
        {
            var voxels = SearchVoxelsInSphere(ray, radius);
            var result = new List<int>();

            foreach (var voxel in voxels)
                result.AddRange(voxel.SearchInSphere(ray, radius));

            return result;
        }

        private List<SearchVoxel> SearchVoxelsInSphere(Vector3 center, float radius)
        {
            var result = new List<SearchVoxel>();

            foreach (var searchVoxel in voxels)
            {
                var closest = searchVoxel.Bounds.ClosestPoint(center);
                if ((closest  - center).sqrMagnitude < radius*radius)
                    result.Add(searchVoxel);
            }

            return result;
        }
        
        private List<SearchVoxel> SearchVoxelsInSphere(Ray ray, float radius)
        {
            var result = new List<SearchVoxel>();
            
            foreach (var searchVoxel in voxels)
            {
                if(searchVoxel.Bounds.IntersectRay(ray))
                    result.Add(searchVoxel);
            }

            return result;
        }

        public void DebugDraw(Transform transform)
        {
            foreach (var searchVoxel in voxels)
            {
                searchVoxel.DebugDraw(transform);
            }
        }
    }
}
