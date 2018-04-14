using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
    public static class TransformUtils
    {
        public static Vector3[] TransformPoints(this Transform transform, Vector3[] points)
        {
            var result = new Vector3[points.Length];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = transform.TransformPoint(points[i]);
            }

            return result;
        }

        public static Vector3[] InverseTransformPoints(this Transform transform, Vector3[] points)
        {
            var result = new Vector3[points.Length];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = transform.InverseTransformPoint(points[i]);
            }

            return result;
        }

        public static void TransformPoints(this Transform transform, ref Vector3[] points)
        {
            for (var i = 0; i < points.Length; i++)
            {
                points[i] = transform.TransformPoint(points[i]);
            }
        }

        public static Vector3[] TransformVectors(this Transform transform, Vector3[] vectors)
        {
            var result = new Vector3[vectors.Length];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = transform.TransformVector(vectors[i]);
            }

            return result;
        }

        public static void TransformVectors(this Transform transform, ref Vector3[] vectors)
        {
            for (var i = 0; i < vectors.Length; i++)
            {
                vectors[i] = transform.TransformVector(vectors[i]);
            }
        }

        public static Vector3[] TransformDirrections(this Transform transform, Vector3[] dirrections)
        {
            var result = new Vector3[dirrections.Length];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = transform.TransformDirection(dirrections[i]);
            }

            return result;
        }

        public static void TransformDirrections(this Transform transform, ref Vector3[] dirrections)
        {
            for (var i = 0; i < dirrections.Length; i++)
            {
                dirrections[i] = transform.TransformDirection(dirrections[i]);
            }
        }

        public static Vector3[] TransformPoints(this Matrix4x4 matrix, Vector3[] points)
        {
            var result = new Vector3[points.Length];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = matrix.MultiplyPoint3x4(points[i]);
            }

            return result;
        }

        public static Vector3[] InverseTransformPoints(this Matrix4x4 matrix, Vector3[] points)
        {
            var result = new Vector3[points.Length];
            var inverseMatrix = matrix.inverse;

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = inverseMatrix.MultiplyPoint3x4(points[i]);
            }

            return result;
        }

    }
}
