using System.Collections;
using UnityEngine;

namespace Assets.GridMap
{
    [CreateAssetMenu(menuName = "Action Based Map Settings/Height Map Generator Settings")]
    [System.Serializable]
    public class ActionBasedHeightMapSettings : ScriptableObject
    {
        [Range(0.9f, 0.999f)]
        public float blobPower = 0.98f;
        [Range(0.75f, 0.93f)]
        public float LinePower = 0.81f;
        [Range(20, 200)]
        public int maxActionRange = 10;
        [Range(0, 100)]
        public int HillDensity = 10;
        [Range(0, 4000)]
        public float maxHillHeight = 50f;
        [Range(0, 8000)]
        public float maxHeight = 500f;

    }
}