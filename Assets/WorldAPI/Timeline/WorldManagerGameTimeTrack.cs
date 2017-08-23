#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

namespace WAPI
{
    [TrackColor(0.8642069f, 0.87f, 0.855f)]
    [TrackClipType(typeof(WorldManagerGameTimeClip))]
    public class WorldManagerGameTimeTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<WorldManagerGameTimeMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
#endif