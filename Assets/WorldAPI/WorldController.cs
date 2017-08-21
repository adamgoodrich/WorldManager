using System;
using UnityEngine;

namespace WAPI
{
    /// <summary>
    /// A sample controller and interface for the world manager
    /// </summary>
    public class WorldController : MonoBehaviour, IWorldApiChangeHandler
    {
        public bool m_allowKeyboardControl = true;
        public float m_timeNow;

        void Awake()
        {
            ConnectToWorldAPI();
        }

        // Update is called once per frame
        void Update()
        {
            WorldManager.Instance.GameTime = DateTime.Now;
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
