#if UNITY_2017_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace WAPI
{
    [Serializable]
    public class WorldManagerFogBehaviour : PlayableBehaviour
    {
        [Range(0f, 1f)]
        public float fogHeightPower;
        public float fogHeightMax;
        [Range(0f, 1f)]
        public float fogDistancePower;
        public float fogDistanceMax;
    }
}
#endif