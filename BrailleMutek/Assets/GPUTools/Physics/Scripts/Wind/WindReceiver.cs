using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Wind
{
    public class WindReceiver
    {
        public Vector3 Vector { get; set; }

        private float angle;

        private readonly WindZone[] winds;
        private readonly Perlin2D perlin = new Perlin2D(566);
        private readonly List<NoiseOctave> octaves = new List<NoiseOctave>(); 

        public WindReceiver()
        {
            winds = Object.FindObjectsOfType<WindZone>();

            octaves.Add(new NoiseOctave(1, 1));
            octaves.Add(new NoiseOctave(5, 0.6f));
            octaves.Add(new NoiseOctave(10, 0.4f));
            octaves.Add(new NoiseOctave(20, 0.3f));
        }

        public Vector3 GetWind(Vector3 position)
        {
            Vector = Vector3.zero;

            foreach (var windZone in winds)
            {
                if (windZone.mode == WindZoneMode.Directional)
                {
                    UpdateDirectionalWind(windZone);
                }
                else
                {
                    UpdateSphericalWind(windZone, position);
                }
            }

            return Vector;
        }

        private void UpdateDirectionalWind(WindZone wind)
        {
            var direction = wind.transform.rotation*Vector3.forward;
            Vector += GetAmplitude(wind, direction);
        }

        private void UpdateSphericalWind(WindZone wind, Vector3 center)
        {
            var diff = center - wind.transform.position;

            if(diff.magnitude > wind.radius)
                return;

            Vector += GetAmplitude(wind, diff.normalized);
        }

        private Vector3 GetAmplitude(WindZone wind, Vector3 dirrection)
        {
            angle += wind.windPulseFrequency;

            var noise = GetNoise(angle);
            var amplitude = wind.windMain + noise * wind.windPulseMagnitude;

            return dirrection*amplitude;
        }

        private float GetNoise(float angle)
        {
            return Mathf.Abs(perlin.Noise(new Vector2(0, angle), octaves));
        }
    }
}
