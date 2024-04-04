using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GridMap
{
    public class SimpleNoiseFilter : INoiseFilter
    {
        NoiseSettings settings;
        Noise noise = new Noise();

        public SimpleNoiseFilter(NoiseSettings settings)
        {
            this.settings = settings;
        }

        public float Evaluate(Vector3 point)
        {
            float noiseValue = 0;
            float frequency = settings.baseRoughness / 100f;
            float amplitude = 1;

            for (int i = 0; i < settings.numLayers; i++)
            {
                float v = noise.Evaluate(point * frequency + settings.center);
                noiseValue += (v + 1) * .5f * amplitude;
                frequency *= settings.roughness;
                amplitude *= settings.persistence;
            }

            noiseValue = Mathf.Max(0,noiseValue-settings.minValue);
            return noiseValue * settings.strength;
        }
    }

    public class RidgidNoiseFilter : INoiseFilter
    {
        NoiseSettings settings;
        Noise noise = new Noise();

        public RidgidNoiseFilter(NoiseSettings settings)
        {
            this.settings = settings;
        }

        public float Evaluate(Vector3 point)
        {
            float noiseValue = 0;
            float frequency = settings.baseRoughness / 100f;
            float amplitude = 1;
            float weight = 1;

            for (int i = 0; i < settings.numLayers; i++)
            {
                float v = 1-Mathf.Abs(noise.Evaluate(point * frequency + settings.center));
                v *= v;
                v *= weight;
                weight = v;
                noiseValue += v * amplitude;
                frequency *= settings.roughness;
                amplitude *= settings.persistence;
            }

            noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
            return noiseValue * settings.strength;
        }
    }

    public interface INoiseFilter
    {
        float Evaluate(Vector3 point);
    }

    public static class NoiseFilterFactory
    {
        public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
        {
            switch (settings.filterType)
            {
                case NoiseSettings.FilterType.Simple:
                    return new SimpleNoiseFilter(settings);
                case NoiseSettings.FilterType.Ridgid:
                    return new RidgidNoiseFilter(settings);
            }
            return null;
        }
    }
}
