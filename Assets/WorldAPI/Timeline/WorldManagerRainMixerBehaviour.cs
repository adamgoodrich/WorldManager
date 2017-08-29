#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Playables;

namespace WAPI
{
    public class WorldManagerRainMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            //Get our inputs
            int inputCount = playable.GetInputCount();

            //Calculate blended values
            float blendedRainPower = 0f;
            float blendedRainPowerOnTerrain = 0f;
            float blendedRainMinHeight = 0f;
            float blendedRainMaxHeight = 0f;
            float totalWeight = 0;

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<WorldManagerRainBehaviour> inputPlayable = (ScriptPlayable<WorldManagerRainBehaviour>) playable.GetInput(i);
                WorldManagerRainBehaviour input = inputPlayable.GetBehaviour();

                blendedRainPower += input.rainPower * inputWeight;
                blendedRainPowerOnTerrain += input.rainPowerOnTerrain * inputWeight;
                blendedRainMinHeight += input.rainMinHeight * inputWeight;
                blendedRainMaxHeight += input.rainMaxHeight * inputWeight;
                totalWeight += inputWeight;
            }

            //We will only update world manager if we got some weights i.e. we are being affected by the timeline
            if (!Mathf.Approximately(totalWeight, 0f))
            {
                Vector4 rainData = WorldManager.Instance.Rain;
                rainData.x = blendedRainPower;
                rainData.y = blendedRainPowerOnTerrain;
                rainData.z = blendedRainMinHeight;
                rainData.w = blendedRainMaxHeight;
                WorldManager.Instance.Rain = rainData;
            }
        }
    }
}
#endif