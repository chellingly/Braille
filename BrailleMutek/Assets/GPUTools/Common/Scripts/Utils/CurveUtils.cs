using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
    public class CurveUtils
    {
        public static Vector3 GetSplinePoint(List<Vector3> points, float t)
        {
            var lastIndex = points.Count - 1;
            int i = (int)(t* points.Count);
            float tStep = 1.0f/points.Count;
            float localT = (t % tStep)*points.Count;

            int y0 = Mathf.Max(0, i - 1);
            int y1 = Mathf.Min(i, lastIndex);
            int y2 = Mathf.Min(i + 1, lastIndex);

            var p0 = points[y0];
            var p1 = points[y1];
            var p2 = points[y2];

            var cPoint1 = (p0 + p1) * 0.5f;
            var cPoint2 = (p1 + p2) * 0.5f;

            return GetBezierPoint(cPoint1, p1, cPoint2, localT);
        }

        public static Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            var invT = 1 - t;
            return invT * invT * p0 + 2 * invT * t * p1 + t * t * p2;
        }

    }
}
