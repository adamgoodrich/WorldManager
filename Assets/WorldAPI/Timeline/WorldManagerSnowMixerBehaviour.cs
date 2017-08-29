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

            //Calculate blended values
            float blendedSnowPower = 0f;
            float blendedSnowPowerOnTerrain = 0f;
            float blendedSnowMinHeight = 0f;
            float blendedSnowAge = 0f;
            float totalWeight = 0;

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<WorldManagerSnowBehaviour> inputPlayable = (ScriptPlayable<WorldManagerSnowBehaviour>) playable.GetInput(i);
                WorldManagerSnowBehaviour input = inputPlayable.GetBehaviour();

                blendedSnowPower += input.snowPower * inputWeight;
                blendedSnowPowerOnTerrain += input.snowPowerOnTerrain * inputWeight;
                blendedSnowMinHeight += input.snowMinHeight * inputWeight;
                blendedSnowAge += input.snowAge * inputWeight;

                totalWeight += inputWeight;
            }

            //We will only update world manager if we got some weights i.e. we are being affected by the timeline
            if (!Mathf.Approximately(totalWeight, 0f))
            {
                Vector4 snowData = WorldManager.Instance.Snow;
                snowData.x = blendedSnowPower;
                snowData.y = blendedSnowPowerOnTerrain;
                snowData.z = blendedSnowMinHeight;
                snowData.w = blendedSnowAge;
                WorldManager.Instance.Snow = snowData;
            }
        }
    }
}
#endif