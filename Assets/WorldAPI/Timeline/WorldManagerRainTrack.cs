#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace WAPI
{
    [TrackColor(0.0f, 0.6f, 0.8f)]
    [TrackClipType(typeof(WorldManagerRainClip))]
    public class WorldManagerRainTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (var c in GetClips())
            {
                c.displayName = "Rain";
            }
            return ScriptPlayable<WorldManagerRainMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
#endif