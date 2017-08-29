#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Playables;

namespace WAPI
{
    public class WorldManagerFogMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            //Get our inputs
            int inputCount = playable.GetInputCount();

            //Calculate blended
            float blendedFogHeightPower = 0f;
            float blendedHeightMax = 0f;
            float blendedFogDistancePower = 0f;
            float blendedFogDistanceMax = 0f;
            float totalWeight = 0;

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<WorldManagerFogBehaviour> inputPlayable = (ScriptPlayable<WorldManagerFogBehaviour>) playable.GetInput(i);
                WorldManagerFogBehaviour input = inputPlayable.GetBehaviour();

                blendedFogHeightPower += input.fogHeightPower * inputWeight;
                blendedHeightMax += input.fogHeightMax * inputWeight;
                blendedFogDistancePower += input.fogDistancePower * inputWeight;
                blendedFogDistanceMax += input.fogDistanceMax * inputWeight;
                totalWeight += inputWeight;
            }

            //We will only update world manager if we got some weights i.e. we are being affected by the timeline
            if (!Mathf.Approximately(totalWeight, 0f))
            {
                Vector4 fogData = WorldManager.Instance.Fog;
                fogData.x = blendedFogHeightPower;
                fogData.y = blendedHeightMax;
                fogData.z = blendedFogDistancePower;
                fogData.w = blendedFogDistanceMax;
                WorldManager.Instance.Fog = fogData;
            }
        }
    }
}
#endif