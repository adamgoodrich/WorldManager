#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace WAPI
{
    [TrackColor(0.8642069f, 0.87f, 0.855f)]
    [TrackMediaType(TimelineAsset.MediaType.Script)]
    [TrackClipType(typeof(WorldManagerFogClip))]
    public class WorldManagerFogTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (var c in GetClips())
            {
                c.displayName = "Fog";
            }
            return ScriptPlayable<WorldManagerFogMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
#endif