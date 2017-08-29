#if UNITY_2017_1_OR_NEWER
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace WAPI
{
    [CustomPropertyDrawer(typeof(WorldManagerRainBehaviour))]
    public class WorldManagerRainDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int fieldCount = 4;
            return fieldCount*EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty rainPowerProperty = property.FindPropertyRelative("rainPower");
            SerializedProperty rainPowerOnTerrainProperty = property.FindPropertyRelative("rainPowerOnTerrain");
            SerializedProperty rainMinHeightProperty = property.FindPropertyRelative("rainMinHeight");
            SerializedProperty rainMaxHeightProperty = property.FindPropertyRelative("rainMaxHeight");

            Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(singleFieldRect, rainPowerProperty);
            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, rainPowerOnTerrainProperty);
            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, rainMinHeightProperty);
            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, rainMaxHeightProperty);
        }
    }
}
#endif