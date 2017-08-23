#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace WAPI
{
    [TrackColor(0.8642069f, 0.87f, 0.855f)]
    [TrackClipType(typeof(WorldManagerSnowMinHeightClip))]
    public class WorldManagerSnowMinHeightTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<WorldManagerSnowMinHeightMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
#endif