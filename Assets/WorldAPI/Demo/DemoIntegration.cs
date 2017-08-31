#if WORLDAPI_PRESENT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WAPI
{
    /// <summary>
    /// A sample class demonstrating integration with World API.
    /// 
    /// Integration is very simple - add the IWorldApiChangeHandler interface,
    /// implement OnWorldChanged and then listen for the messages you want to hear, 
    /// or alternatively call the World API methods you are interested in via 
    /// the generic WorldManager.Instance.APIYourCalling();
    /// 
    /// To control message receipt from WAPI you can use the AddListener and 
    /// RemoveListener methods.
    /// 
    /// NOTE: When present in a project WAPI will inject the WORLDAPI_PRESENT
    /// symbol, so you can wrap your class in am #if statement like this one
    /// is in order to avoid compilation errors if WAPI is missing in the 
    /// target project.
    /// 
    /// </summary>
    public class DemoIntegration : MonoBehaviour, IWorldApiChangeHandler
    {
        [Header("UX Text")]
        public Text m_gameTime;
        public Text m_fogHeightPower;
        public Text m_fogHeightMax;
        public Text m_fogDistancePower;
        public Text m_fogDistanceMax;

        //Initialization
        void Start()
        {
            ConnectToWorldAPI();
        }

        //Update is called once per frame
        void Update()
        {
        }

        //Disconnect from WAPI
        void OnDestroy()
        {
            DisconnectFromWorldAPI();
        }

        #region World Manager API Integration

        /// <summary>
        /// Start listening to world api messaged
        /// </summary>
        void ConnectToWorldAPI()
        {
            WorldManager.Instance.AddListener(this);
        }

        /// <summary>
        /// Stop listening to world api messages
        /// </summary>
        void DisconnectFromWorldAPI()
        {
            WorldManager.Instance.RemoveListener(this);
        }

        /// <summary>
        /// This method is called when the class has been added as a listener, and something has changed 
        /// one of the WAPI settings.
        /// 
        /// Use the HasChanged method to work out what was changed and respond accordingly. 
        /// 
        /// NOTE : As the majority of the World API values are properties, setting something 
        /// is as easy as reading its value, and setting a property will cause another
        /// OnWorldChanged event to be raised.
        /// 
        /// </summary>
        /// <param name="changeArgs"></param>
        public void OnWorldChanged(WorldChangeArgs changeArgs)
        {
            if (changeArgs.HasChanged(WorldConstants.WorldChangeEvents.GameTimeChanged))
            {
                if (m_gameTime != null)
                {
                    m_gameTime.text = string.Format("Game Time : {0}", WorldManager.Instance.GetTimeDecimal());
                }
            }
            else if (changeArgs.HasChanged(WorldConstants.WorldChangeEvents.FogChanged))
            {
                if (m_fogHeightPower != null)
                {
                    m_fogHeightPower.text = string.Format("Fog Height Power : {0}", WorldManager.Instance.FogHeightPower);
                }
                if (m_fogHeightMax != null)
                {
                    m_fogHeightMax.text = string.Format("Fog Height Max : {0}", WorldManager.Instance.FogHeightMax);
                }
                if (m_fogDistancePower != null)
                {
                    m_fogDistancePower.text = string.Format("Fog Distance Power : {0}", WorldManager.Instance.FogDistancePower);
                }
                if (m_fogDistanceMax != null)
                {
                    m_fogDistanceMax.text = string.Format("Fog Distance Max : {0}", WorldManager.Instance.FogDistanceMax);
                }
            }
        }

        #endregion
    }
}
#endif