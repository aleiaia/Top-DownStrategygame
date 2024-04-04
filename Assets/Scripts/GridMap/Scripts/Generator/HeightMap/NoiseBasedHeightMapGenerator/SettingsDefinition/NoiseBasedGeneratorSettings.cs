using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.GridMap
{
    [CreateAssetMenu(menuName = "Noise Based Map Settings/Generator Settings")]
    [System.Serializable]
    public class NoiseBasedGeneratorSettings : ScriptableObject
    {
        [HideInInspector]
        public bool heightMapGeneratorSettingsFoldout;
        public NoiseBasedHeightMapSettings heighMapGeneratorSettings;
    }
}