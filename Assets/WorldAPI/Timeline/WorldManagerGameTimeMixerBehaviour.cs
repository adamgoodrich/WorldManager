#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Playables;

namespace WAPI
{
    public class WorldManagerGameTimeMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            //Get our inputs
            int inputCount = playable.GetInputCount();

            //Calculate blended game time
            double blendedGameTime = 0;
            double totalWeight = 0;
            for (int i = 0; i < inputCount; i++)
            {
                double inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<WorldManagerGameTimeBehaviour> inputPlayable =
                    (ScriptPlayable<WorldManagerGameTimeBehaviour>) playable.GetInput(i);
                WorldManagerGameTimeBehaviour input = inputPlayable.GetBehaviour();

                blendedGameTime += input.gameTime*inputWeight;
                totalWeight += inputWeight;
            }

            //We will only update world manager if we got some weights i.e. we are being affected by the timeline
            if (!Mathf.Approximately((float) totalWeight, 0f))
            {
                //Debug.Log("TW " + totalWeight + " " + blendedGameTime);
                WorldManager.Instance.SetDecimalTime(blendedGameTime);
            }
        }
    }
}
#endif