#if UNITY_2017_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace WAPI
{
    [Serializable]
    public class WorldManagerSnowClip : PlayableAsset, ITimelineClipAsset
    {
        public WorldManagerSnowBehaviour template = new WorldManagerSnowBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<WorldManagerSnowBehaviour>.Create(graph, template);
            return playable;
        }
    }
}
#endif