#if UNITY_2017_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace WAPI
{
    [Serializable]
    public class WorldManagerRainBehaviour : PlayableBehaviour
    {
        [Range(0f, 1f)]
        public float rainPower;
        [Range(0f, 1f)]
        public float rainPowerOnTerrain;
        public float rainMinHeight;
        public float rainMaxHeight;
    }
}
#endif