#if UNITY_2017_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace WAPI
{
    [Serializable]
    public class WorldManagerGameTimeBehaviour : PlayableBehaviour
    {
        public double gameTime;
    }
}
#endif