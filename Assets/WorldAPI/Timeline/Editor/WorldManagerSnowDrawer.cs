#if UNITY_2017_1_OR_NEWER
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace WAPI
{
    [CustomPropertyDrawer(typeof(WorldManagerSnowBehaviour))]
    public class WorldManagerSnowDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int fieldCount = 4;
            return fieldCount*EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty snowPowerProperty = property.FindPropertyRelative("snowPower");
            SerializedProperty snowPowerOnTerrainProperty = property.FindPropertyRelative("snowPowerOnTerrain");
            SerializedProperty snowMinHeightProperty = property.FindPropertyRelative("snowMinHeight");
            SerializedProperty snowAgeProperty = property.FindPropertyRelative("snowAge");

            Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(singleFieldRect, snowPowerProperty);
            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, snowPowerOnTerrainProperty);
            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, snowMinHeightProperty);
            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, snowAgeProperty);
        }
    }
}
#endif