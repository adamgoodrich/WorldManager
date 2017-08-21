using System;
using UnityEngine;

namespace WAPI
{
    /// <summary>
    /// A sample controller and interface for the world manager
    /// </summary>
    public class WorldController : MonoBehaviour, IWorldApiChangeHandler
    {
        public bool m_allowKeyboardControl = false;
        public float m_timeUpdateIncrement = 0.25f;
        public float m_timeNow;

        void Awake()
        {
            ConnectToWorldAPI();
        }

        // Update is called once per frame
        void Update()
        {
            if (!m_allowKeyboardControl)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                m_timeNow -= m_timeUpdateIncrement;
                if (m_timeNow < 0f)
                {
                    m_timeNow = 23.99f;
                }
                WorldManager.Instance.SetDecimalTime(m_timeNow);
            }

            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                m_timeNow += m_timeUpdateIncrement;
                if (m_timeNow > 23.99f)
                {
                    m_timeNow = 0.0f;
                }
                WorldManager.Instance.SetDecimalTime(m_timeNow);
            }

//            if (Input.GetKeyDown(KeyCode.P))
//            {
//                m_currentWeather += 1;
//                if (m_currentWeather >= m_maxWeatherID)
//                {
//                    m_currentWeather = 0;
//                }
//                m_enviroSky.ChangeWeather(m_currentWeather);
//            }
        }

        void OnDestroy()
        {
            DisconnectFromWorldAPI();
        }

        void ConnectToWorldAPI()
        {
            WorldManager.Instance.AddListener(this);            
        }

        void DisconnectFromWorldAPI()
        {
            WorldManager.Instance.RemoveListener(this);
        }

        public void OnWorldChanged(WorldChangeArgs changeArgs)
        {
            if (changeArgs.HasChanged(WorldConstants.WorldChangeEvents.GameTimeChanged))
            {
                m_timeNow = (float)changeArgs.manager.GetTimeDecimal();
            }
        }
    }
}
