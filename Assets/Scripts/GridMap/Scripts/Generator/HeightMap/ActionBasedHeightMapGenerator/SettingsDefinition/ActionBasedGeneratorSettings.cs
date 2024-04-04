using System.Collections;
using UnityEngine;

namespace Assets.GridMap
{
    [CreateAssetMenu(menuName = "Action Based Map Settings/Generator Settings")]
    [System.Serializable]
    public class ActionBasedGeneratorSettings : ScriptableObject
    {
        [HideInInspector]
        public bool heightMapGeneratorSettingsFoldout;
        public ActionBasedHeightMapSettings heighMapGeneratorSettings;
    }
}