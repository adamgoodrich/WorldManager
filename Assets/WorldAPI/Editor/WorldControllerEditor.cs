using UnityEditor;
using UnityEngine;

namespace WorldAPI
{
    /// <summary>
    /// Editor script for world API sample controller
    /// </summary>
    [CustomEditor(typeof(WorldController))]
    public class WorldControllerEditor : Editor
    {
        private GUIStyle m_boxStyleNormal;
        private GUIStyle m_boxStyle;
        private GUIStyle m_wrapStyle;
        private GUIStyle m_wrapHelpStyle;
        private GUIStyle m_descWrapStyle;
        private bool m_showTooltips = true;
        private bool m_globalHelp = false;
        private WorldController m_controller;

        /// <summary>
        /// Called when we select this object
        /// </summary>
        void OnEnable()
        {
            //Check for target
            if (target == null)
            {
                return;
            }

            //Setup target
            m_controller = (WorldController)target;
        }




    }
}