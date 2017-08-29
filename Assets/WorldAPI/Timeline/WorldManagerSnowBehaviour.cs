#if UNITY_2017_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace WAPI
{
    [Serializable]
    public class WorldManagerSnowBehaviour : PlayableBehaviour
    {
        [Range(0f, 1f)]
        public float snowPower;
        [Range(0f, 1f)]
        public float snowPowerOnTerrain;
        public float snowMinHeight;
        public float snowAge;
    }
}
#endif