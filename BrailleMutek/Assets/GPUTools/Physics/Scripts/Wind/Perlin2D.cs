using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace GPUTools.Physics.Scripts.Wind
{
    public class Perlin2D
    {
        private readonly byte[] permutationTable;

        static float QunticCurve(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        public Perlin2D(int seed = 0)
        {
            var rand = new Random(seed);
            permutationTable = new byte[1024];
            rand.NextBytes(permutationTable);
        }

        private Vector2 GetPseudoRandomGradientVector(int x, int y)
        {
            var v = (int)(((x * 1836311903) ^ (y * 2971215073) + 4807526976) & 1023);
            v = permutationTable[v] & 3;

            switch (v)
            {
                case 0:
                    return new Vector2(1, 0);
                case 1:
                    return new Vector2(-1, 0);
                case 2:
                    return new Vector2(0, 1);
                default:
                    return new Vector2(0, -1);
            }
        }

        public float Noise(Vector2 fp)
        {
            var left = (int)Math.Floor(fp.x);
            var top = (int)Math.Floor(fp.y);
            var pointInQuadX = fp.x - left;
            var pointInQuadY = fp.y - top;

            var topLeftGradient = GetPseudoRandomGradientVector(left, top);
            var topRightGradient = GetPseudoRandomGradientVector(left + 1, top);
            var bottomLeftGradient = GetPseudoRandomGradientVector(left, top + 1);
            var bottomRightGradient = GetPseudoRandomGradientVector(left + 1, top + 1);

            var distanceToTopLeft = new Vector2(pointInQuadX, pointInQuadY);
            var distanceToTopRight = new Vector2(pointInQuadX - 1, pointInQuadY);
            var distanceToBottomLeft = new Vector2(pointInQuadX, pointInQuadY - 1);
            var distanceToBottomRight = new Vector2(pointInQuadX - 1, pointInQuadY - 1);

            var tx1 = Vector3.Dot(distanceToTopLeft, topLeftGradient);
            var tx2 = Vector3.Dot(distanceToTopRight, topRightGradient);
            var bx1 = Vector3.Dot(distanceToBottomLeft, bottomLeftGradient);
            var bx2 = Vector3.Dot(distanceToBottomRight, bottomRightGradient);

            pointInQuadX = QunticCurve(pointInQuadX);
            pointInQuadY = QunticCurve(pointInQuadY);

            var tx = Mathf.Lerp(tx1, tx2, pointInQuadX);
            var bx = Mathf.Lerp(bx1, bx2, pointInQuadX);
            var tb = Mathf.Lerp(tx, bx, pointInQuadY);

            return tb;
        }

        public float Noise(Vector2 fp, int octaves, float persistence = 0.5f)
        {
            var amplitude = 1f;
            var max = 0f;
            var result = 0f;

            while (octaves-- > 0)
            {
                max += amplitude;
                result += Noise(fp) * amplitude;
                amplitude *= persistence;
                fp.x *= 2;
                fp.y *= 2;
            }

            return result / max;
        }

        public float Noise(Vector2 fp, List<NoiseOctave> octaves)
        {
            var result = 0f;
            var max = 0f;

            for (int i = 0; i < octaves.Count; i++)
            {
                var octave = octaves[i];
                max += octave.Amplitude;
                result += Noise(fp*octave.Scale)*octave.Amplitude;
            }

            return result / max;
        }

       
    }

    [Serializable]
    public struct NoiseOctave
    {
        public float Scale;
        public float Amplitude;

        public NoiseOctave(float scale, float amplitude)
        {
            Scale = scale;
            Amplitude = amplitude;
        }
    }
}
