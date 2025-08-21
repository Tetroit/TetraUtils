using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TetraUtils
{
    [CustomPropertyDrawer(typeof(KeyboardMap.BooleanBind))]
    public class KeyboardBindDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw without label and padding
            EditorGUI.BeginProperty(position, label, property);

            var nameProp = property.FindPropertyRelative("name");
            var valueProp = property.FindPropertyRelative("binds");

            int binds = valueProp.arraySize;

            // Divide the space into two parts
            float padding = 0;
            Rect nameRect = new Rect(position.x, position.y, position.width/2 - 5, position.height);
            padding += position.width/2;
            Rect[] bindFields = new Rect[binds];

            float addButtonWidth = 25f;
            var enumDist = (position.width - padding - addButtonWidth * 2) / binds;
            for (int i = 0; i < binds; i++)
            {
                bindFields[i] = new Rect(position.x + padding , position.y, enumDist - 5, position.height);
                padding += enumDist;
            }

            EditorGUI.PropertyField(nameRect, nameProp, GUIContent.none);
            for (int i=0; i < binds; i++)
                EditorGUI.PropertyField(bindFields[i], valueProp.GetArrayElementAtIndex(i), GUIContent.none);

            if (GUI.Button(new Rect(position.xMax - addButtonWidth, position.y, addButtonWidth - 5, position.height), new GUIContent("+")))
            {
                var listProp = property.FindPropertyRelative("binds");
                listProp.InsertArrayElementAtIndex(listProp.arraySize);
            }
            if (GUI.Button(new Rect(position.xMax - addButtonWidth * 2, position.y, addButtonWidth - 5, position.height), new GUIContent("-")))
            {
                var listProp = property.FindPropertyRelative("binds");
                listProp.DeleteArrayElementAtIndex(listProp.arraySize-1);
            }

            EditorGUI.EndProperty();
        }
    }
}
