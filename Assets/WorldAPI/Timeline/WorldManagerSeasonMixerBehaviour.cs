#if UNITY_2017_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace WAPI
{
    public class WorldManagerSeasonMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            //Get our inputs
            int inputCount = playable.GetInputCount();

            //Calculate blended season
            double blendedSeason = 0;
            double totalWeight = 0;
            for (int i = 0; i < inputCount; i++)
            {
                double inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<WorldManagerSeasonBehaviour> inputPlayable =
                    (ScriptPlayable<WorldManagerSeasonBehaviour>) playable.GetInput(i);
                WorldManagerSeasonBehaviour input = inputPlayable.GetBehaviour();

                blendedSeason += input.season*inputWeight;
                totalWeight += inputWeight;
            }

            //We will only update world manager if we got some weights i.e. we are being affected by the timeline
            if (!Mathf.Approximately((float) totalWeight, 0f))
            {
                WorldManager.Instance.Season = (float) blendedSeason;
            }
        }
    }
}
#endif