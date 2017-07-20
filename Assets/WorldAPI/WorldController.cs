using System;
using UnityEngine;

namespace WorldAPI
{
    /// <summary>
    /// A sample controller and interface for the world manager
    /// </summary>
    public class WorldController : MonoBehaviour
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
            //DisconnectFromWorldAPI();
        }

        void ConnectToWorldAPI()
        {
            WorldManager.Instance.OnGameTimeChanged += OnGameTimeChanged;
        }

        void DisconnectFromWorldAPI()
        {
            WorldManager.Instance.OnGameTimeChanged -= OnGameTimeChanged;
        }

        public void OnGameTimeChanged(WorldManager wm, DateTime newTime)
        {
            m_timeNow = newTime.Hour + (newTime.Minute / 60f) + (newTime.Second / 3600f);
        }

    }
}
