#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Playables;

namespace WAPI
{
    public class WorldManagerSnowMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            //Get our inputs
            int inputCount = playable.GetInputCount();

            //Calculate blended snow power
            double blendedSnowPower = 0;
            double totalWeight = 0;
            for (int i = 0; i < inputCount; i++)
            {
                double inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<WorldManagerSnowBehaviour> inputPlayable =
                    (ScriptPlayable<WorldManagerSnowBehaviour>) playable.GetInput(i);
                WorldManagerSnowBehaviour input = inputPlayable.GetBehaviour();

                blendedSnowPower += input.snowPower*inputWeight;
                totalWeight += inputWeight;
            }

            //We will only update world manager if we got some weights i.e. we are being affected by the timeline
            if (!Mathf.Approximately((float) totalWeight, 0f))
            {
                WorldManager.Instance.SnowPower = (float) blendedSnowPower;
            }
        }
    }
}
#endif