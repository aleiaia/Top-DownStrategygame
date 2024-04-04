using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GridMap
{
    [CreateAssetMenu(menuName = "Noise Based Map Settings/Height Map Generator Settings")]
    public class NoiseBasedHeightMapSettings : ScriptableObject
    {
        public float minElavation = 0f;
        public float maxElevetion = 100f;
        public NoiseLayer[] noiseLayers;
    }

    [System.Serializable]
    public class NoiseLayer
    {
        public bool useFirsLayerAsMask;
        public bool enabled = true;
        public NoiseSettings noiseSettings;
    }

    [System.Serializable]
    public class NoiseSettings
    {
        public enum FilterType { Simple, Ridgid }
        public FilterType filterType;

        [Range(1, 8)]
        public int numLayers = 1;

        public float strength;
        public float baseRoughness = 1;
        [Range(1f,100f)]
        public float roughness = 2;
        [Range(0f,1f)]
        public float persistence = .5f;
        public Vector3 center;
        public float minValue;
    }
}