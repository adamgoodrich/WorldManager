using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WAPI
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
        #pragma warning disable 414
        private WorldController m_controller;
        #pragma warning restore 414

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

        /// <summary>
        /// Editor UX
        /// </summary>
        public override void OnInspectorGUI()
        {
            #region Setup and introduction

            //Get the target
            m_controller = (WorldController) target;

            //Set up the styles
            if (m_boxStyle == null)
            {
                m_boxStyle = new GUIStyle(GUI.skin.box);
                m_boxStyle.normal.textColor = GUI.skin.label.normal.textColor;
                m_boxStyle.fontStyle = FontStyle.Bold;
                m_boxStyle.alignment = TextAnchor.UpperLeft;
            }

            if (m_wrapStyle == null)
            {
                m_wrapStyle = new GUIStyle(GUI.skin.label);
                m_wrapStyle.fontStyle = FontStyle.Normal;
                m_wrapStyle.wordWrap = true;
            }

            if (m_wrapHelpStyle == null)
            {
                m_wrapHelpStyle = new GUIStyle(GUI.skin.label);
                m_wrapHelpStyle.richText = true;
                m_wrapHelpStyle.wordWrap = true;
            }

            //Text intro
            GUILayout.BeginVertical(string.Format("WorldManager ({0}.{1})", WorldConstants.MajorVersion, WorldConstants.MinorVersion), m_boxStyle);
            if (m_globalHelp)
            {
                Rect rect = EditorGUILayout.BeginVertical();
                rect.x = rect.width - 10;
                rect.width = 25;
                rect.height = 20;
                if (GUI.Button(rect, "?-"))
                {
                    m_globalHelp = !m_globalHelp;
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                Rect rect = EditorGUILayout.BeginVertical();
                rect.x = rect.width - 10;
                rect.width = 25;
                rect.height = 20;
                if (GUI.Button(rect, "?+"))
                {
                    m_globalHelp = !m_globalHelp;
                }
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(20);

            EditorGUILayout.LabelField("Welcome to World Manager Controller. Click ? for help.", m_wrapStyle);
            DrawHelpSectionLabel("Overview");

            if (m_globalHelp)
            {
                if (GUILayout.Button(GetLabel("View Online Forum")))
                {
                    Application.OpenURL("https://forum.unity3d.com/threads/world-manager-generic-world-management-system.484239/");
                }
            }

            GUILayout.EndVertical();

            #endregion

            //Monitor for changes
            EditorGUI.BeginChangeCheck();

            GUILayout.BeginVertical("Control", m_boxStyle);
            GUILayout.Space(20f);
            DrawHelpSectionLabel("Control");

            GUILayout.BeginVertical(m_boxStyle);

            float decimalTime = EditorGUILayout.Slider(GetLabel("Time"), (float)WorldManager.Instance.GetTimeDecimal(), 0f, 24f);
            DrawHelpLabel("Time");

            float rainPower = EditorGUILayout.Slider(GetLabel("Rain Power"), WorldManager.Instance.RainPower, 0f, 1f);
            DrawHelpLabel("Rain Power");

            float snowPower = EditorGUILayout.Slider(GetLabel("Snow Power"), WorldManager.Instance.SnowPower, 0f, 1f);
            DrawHelpLabel("Snow Power");

            float season = EditorGUILayout.Slider(GetLabel("Season"), WorldManager.Instance.Season, 0f, 3.9999f);
            DrawHelpLabel("Season");

            EditorGUI.indentLevel++;
            if (season< 1f)
            {
                EditorGUILayout.LabelField(string.Format("{0:0}% Winter {1:0}% Spring", (1f - season) * 100f, season* 100f));
            }
            else if (season< 2f)
            {
                EditorGUILayout.LabelField(string.Format("{0:0}% Spring {1:0}% Summer", (2f - season) * 100f, (season - 1f) * 100f));
            }
            else if (season< 3f)
            {
                EditorGUILayout.LabelField(string.Format("{0:0}% Summer {1:0}% Autumn", (3f - season) * 100f, (season - 2f) * 100f));
            }
            else 
            {
                EditorGUILayout.LabelField(string.Format("{0:0}% Autumn {1:0}% Winter", (4f - season) * 100f, (season - 3f) * 100f));
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();

            GUILayout.EndVertical();


            GUILayout.BeginVertical("Settings", m_boxStyle);
            GUILayout.Space(20f);
            DrawHelpSectionLabel("Settings");

            GUILayout.BeginVertical(m_boxStyle);
            float minSnowHeight = EditorGUILayout.FloatField(GetLabel("Min Snow Height"), WorldManager.Instance.SnowMinHeight);
            DrawHelpLabel("Min Snow Height");

/*
            float maxSmoothness = EditorGUILayout.Slider(GetLabel("Max Smoothness"), m_manager.MaxRainSmoothness, 0f, 30f);
            DrawHelpLabel("Max Smoothness");

            Color winterTint = EditorGUILayout.ColorField(GetLabel("Winter Tint"), m_manager.WinterTint, true, false, false, null);
            DrawHelpLabel("Winter Tint");

            Color springTint = EditorGUILayout.ColorField(GetLabel("Spring Tint"), m_manager.SpringTint, true, false, false, null);
            DrawHelpLabel("Spring Tint");

            Color summerTint = EditorGUILayout.ColorField(GetLabel("Summer Tint"), m_manager.SummerTint, true, false, false, null);
            DrawHelpLabel("Summer Tint");

            Color autumnTint = EditorGUILayout.ColorField(GetLabel("Autumn Tint"), m_manager.AutumnTint, true, false, false, null);
            DrawHelpLabel("Autumn Tint");
*/

            GUILayout.EndVertical();

            GUILayout.EndVertical();

            //Handle changes
            if (EditorGUI.EndChangeCheck())
            {
                //CompleteTerrainShader.SetDirty(m_manager);

                //UX Settings
                WorldManager.Instance.SnowPower = snowPower;
                WorldManager.Instance.SnowMinHeight = minSnowHeight;
                WorldManager.Instance.RainPower = rainPower;
                WorldManager.Instance.Season = season;
            }
        }

        /// <summary>
        /// Draw some help
        /// </summary>
        /// <param name="title"></param>
        private void DrawHelpSectionLabel(string title)
        {
            if (m_globalHelp)
            {
                string description;
                if (m_tooltips.TryGetValue(title, out description))
                {
                    GUILayout.BeginVertical(m_boxStyle);
                    if (EditorGUIUtility.isProSkin)
                    {
                        EditorGUILayout.LabelField(string.Format("<color=#CBC5C1><b>{0}</b>\n\n{1}\n</color>", title, description), m_wrapHelpStyle);
                    }
                    else
                    {
                        EditorGUILayout.LabelField(string.Format("<color=#3F3D40><b>{0}</b>\n\n{1}\n</color>", title, description), m_wrapHelpStyle);
                    }
                    GUILayout.EndVertical();
                }
            }
        }

        /// <summary>
        /// Draw some help
        /// </summary>
        /// <param name="title"></param>
        private void DrawHelpLabel(string title)
        {
            if (m_globalHelp)
            {
                string description;
                if (m_tooltips.TryGetValue(title, out description))
                {
                    //EditorGUILayout.LabelField(string.Format("<color=lightblue><b>{0}</b>\n{1}</color>", title, description), m_wrapHelpStyle);
                    EditorGUI.indentLevel++;
                    if (EditorGUIUtility.isProSkin)
                    {
                        EditorGUILayout.LabelField(string.Format("<color=#98918F>{0}</color>", description), m_wrapHelpStyle);
                    }
                    else
                    {
                        EditorGUILayout.LabelField(string.Format("<color=#6F6C6F>{0}</color>", description), m_wrapHelpStyle);
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }

        /// <summary>
        /// Get a content label - look the tooltip up if possible
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        GUIContent GetLabel(string name)
        {
            string tooltip = "";
            if (m_showTooltips)
            {
                if (m_tooltips.TryGetValue(name, out tooltip))
                {
                    return new GUIContent(name, tooltip);
                }
                else
                {
                    return new GUIContent(name);
                }
            }
            else
            {
                return new GUIContent(name);
            }
        }

        /// <summary>
        /// The tooltips
        /// </summary>
        static Dictionary<string, string> m_tooltips = new Dictionary<string, string>
        {
            { "Overview", "    The World Manager controller provides a simple interface test and control your world."},
            { "Control", "    This section allows you to control your system."},
            { "Settings", "    This section allows you to update settings."},
            { "Rain Power", "The power of the rain. This modifies texture smoothness to simulate the effect of rain."},
            { "Snow Power", "The power of the snow. This controls the strength of the snow setting applied."},
            { "Season", "The season controls the tint applied to the terrain textures to simulate seasonal shifts. Note: This tint will overwrite the any tint that may have been configured in the profile."},
            { "Min Snow Height", "The minimum height from which snow will be applied."},
        };
    }
}