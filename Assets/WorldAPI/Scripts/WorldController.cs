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

        //Start oriented settings
        public bool m_applyStartSettingsOnAwake = true;
        public float m_startGameTime = 8f;
        public float m_startSnowAmount = 0f;
        public float m_startSnowMinHeight = 50f;

        //Temporary settings
        public float m_timeNow;


        public void ApplyStartSettings()
        {
            m_timeNow = m_startGameTime;
            WorldManager.Instance.SetDecimalTime(m_startGameTime);
            WorldManager.Instance.SnowPower = m_startSnowAmount;
            WorldManager.Instance.SnowMinHeight = m_startSnowMinHeight;
        }

        void Awake()
        {
        }

        void Start()
        {
            ConnectToWorldAPI();
            if (m_applyStartSettingsOnAwake)
            {
                ApplyStartSettings();
            }
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
