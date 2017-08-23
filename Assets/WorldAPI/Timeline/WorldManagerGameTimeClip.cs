#if UNITY_2017_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace WAPI
{
    [Serializable]
    public class WorldManagerGameTimeClip : PlayableAsset, ITimelineClipAsset
    {
        public WorldManagerGameTimeBehaviour template = new WorldManagerGameTimeBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<WorldManagerGameTimeBehaviour>.Create(graph, template);
            return playable;
        }
    }
}
#endif