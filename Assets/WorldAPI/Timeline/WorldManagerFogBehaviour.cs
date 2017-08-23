#if UNITY_2017_1_OR_NEWER
using System;
using UnityEngine.Playables;

namespace WAPI
{
    [Serializable]
    public class WorldManagerFogBehaviour : PlayableBehaviour
    {
        public float fogPower;
    }
}
#endif