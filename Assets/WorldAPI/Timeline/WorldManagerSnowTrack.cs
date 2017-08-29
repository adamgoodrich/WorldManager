#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

namespace WAPI
{
    [TrackColor(1f, 1f, 1f)]
    [TrackClipType(typeof(WorldManagerSnowClip))]
    public class WorldManagerSnowTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (var c in GetClips())
            {
                c.displayName = "Snow";
            }
            return ScriptPlayable<WorldManagerSnowMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
#endif