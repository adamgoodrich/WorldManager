#if UNITY_2017_1_OR_NEWER
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace WAPI
{
    [CustomPropertyDrawer(typeof(WorldManagerFogBehaviour))]
    public class WorldManagerFogDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int fieldCount = 4;
            return fieldCount*EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty fogHeightPowerProp = property.FindPropertyRelative("fogHeightPower");
            SerializedProperty fogHeightMaxProp = property.FindPropertyRelative("fogHeightMax");
            SerializedProperty fogDistancePowerProp = property.FindPropertyRelative("fogDistancePower");
            SerializedProperty fogDistanceMax = property.FindPropertyRelative("fogDistanceMax");

            Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(singleFieldRect, fogHeightPowerProp);
            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, fogHeightMaxProp);
            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, fogDistancePowerProp);
            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(singleFieldRect, fogDistanceMax);

        }
    }
}
#endif