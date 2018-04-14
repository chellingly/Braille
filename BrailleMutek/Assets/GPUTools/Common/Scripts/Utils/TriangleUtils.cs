using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
    public class TriangleUtils
    {
        public static Rect FindBoundRect(Vector2[] points)
        {
            var rect = new Rect
            {
                min = points[0],
                max = points[0]
            };

            for (var i = 1; i < points.Length; ++i)
            {
                if ( points[i].x < rect.min.x ) rect.min = new Vector2(points[i].x, rect.min.y);
                if ( points[i].y < rect.min.y ) rect.min = new Vector2(rect.min.x, points[i].y);

                if ( points[i].x > rect.max.x ) rect.max = new Vector2(points[i].x, rect.max.y);
                if ( points[i].y > rect.max.y ) rect.max = new Vector2(rect.max.x, points[i].y);
            }
         
            return rect;
        }

        public static bool IsPointInsideTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            var barycentric = GetBarycentricInsideTriangle(p, a, b, c);
            return barycentric.x >= 0 && barycentric.y >= 0 && barycentric.x + barycentric.y <= 1;
        }

        public static bool IsPointInsideTriangle(Vector2 barycentric)
        {
            return barycentric.x >= 0 && barycentric.y >= 0 && barycentric.x + barycentric.y <= 1;
        }

        public static Vector3 GetPointInsideTriangle(Vector3 a, Vector3 b, Vector3 c, Vector2 barycentric)
        {
            return a*(1 - (barycentric.x + barycentric.y)) + b*barycentric.y + c*barycentric.x;
        }

        public static Vector2 GetBarycentricInsideTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            // Compute vectors        
            var v0 = c - a;
            var v1 = b - a;
            var v2 = p - a;

            // Compute dot products
            var dot00 = Vector2.Dot(v0, v0);
            var dot01 = Vector2.Dot(v0, v1);
            var dot02 = Vector2.Dot(v0, v2);
            var dot11 = Vector2.Dot(v1, v1);
            var dot12 = Vector2.Dot(v1, v2);

            // Compute barycentric coordinates
            var invDenom = 1f / (dot00 * dot11 - dot01 * dot01);
            var u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            var v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            return new Vector2(u, v);
        }
    }
}