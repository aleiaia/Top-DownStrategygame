using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GridMap
{
    public class NoiseBasedHeightMapGenerator
    {
        NoiseBasedHeightMapSettings settings;
        INoiseFilter[] noiseFilters;
        Noise noise = new Noise();

        public NoiseBasedHeightMapGenerator(NoiseBasedHeightMapSettings settings)
        {
            this.settings = settings;
            noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
            for (int i = 0; i < settings.noiseLayers.Length; i++)
            {
                noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
            }
        }

        public float CalculateHeightOnPoint(Vector3 point)
        {
            float firstLayerValue = 0;
            float elevation = 0;
            if (noiseFilters.Length > 0)
            {
                firstLayerValue = noiseFilters[0].Evaluate(point);
                if (settings.noiseLayers[0].enabled)
                {
                    elevation = firstLayerValue;
                }
            }

            for (int i = 0; i < noiseFilters.Length; i++)
            {
                if (settings.noiseLayers[i].enabled)
                {
                    float mask = (settings.noiseLayers[i].useFirsLayerAsMask) ? firstLayerValue : 1;
                    elevation += noiseFilters[i].Evaluate(point) * mask;
                }
            }
            return elevation;
            //return Mathf.Lerp(settings.minElavation,settings.maxElevetion,elevation);
        }
    }


}