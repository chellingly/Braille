using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.SpatialSearch
{
    public class SearchVoxel
    {
        private readonly List<Vector3> vertices;
        private readonly List<int> mineIndices;
        private readonly Bounds bounds;

        public SearchVoxel(List<Vector3> vertices, Bounds bounds)
        {
            this.bounds = bounds;
            this.vertices = vertices;

            mineIndices = SearchMineIndices();
        }

        private List<int> SearchMineIndices()
        {
            var result = new List<int>();

            for (var i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];

                if(bounds.Contains(vertex))
                    result.Add(i);
            }

            return result;
        }

        public List<int> SearchInSphere(Vector3 center, float radius)
        {
            var result = new List<int>();

            foreach (var mineIndex in mineIndices)
            {
                var vertex = vertices[mineIndex];
                if ((vertex - center).sqrMagnitude < radius*radius)
                    result.Add(mineIndex);
            }

            return result;
        }

        public List<int> SearchInSphere(Ray ray, float radius)
        {
            var result = new List<int>();

            foreach (var mineIndex in mineIndices)
            {
                var vertex = vertices[mineIndex];
                var distance = Vector3.Cross(ray.direction, vertex - ray.origin).sqrMagnitude;
                if (distance < radius*radius)
                    result.Add(mineIndex);
            }

            return result;
        }

        public void DebugDraw(Transform transforms)
        {

        }

        public Bounds Bounds
        {
            get { return bounds; }
        }

        public int TotalVertices
        {
            get { return mineIndices.Count; }
        }
    }
}
