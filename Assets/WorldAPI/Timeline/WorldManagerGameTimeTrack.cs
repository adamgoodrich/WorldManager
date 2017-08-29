#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

namespace WAPI
{
    [TrackColor(0f, 0.6f, 0f)]
    [TrackClipType(typeof(WorldManagerGameTimeClip))]
    public class WorldManagerGameTimeTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (var c in GetClips())
            {
                c.displayName = "Time";
            }
            return ScriptPlayable<WorldManagerGameTimeMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
#endif