using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TetraUtils
{
    public static class TagUtils
    {
        public static void AddTag(string tagName)
        {

            // Load TagManager asset
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // Check if tag already exists
            if (HasTag(tagName, tagsProp, true)) return;

            // Add new tag
            tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
            SerializedProperty newTag = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
            newTag.stringValue = tagName;

            tagManager.ApplyModifiedProperties();
            Debug.Log("Tag added: " + tagName);
        }

        public static bool HasTag(string tag, SerializedProperty tags = null, bool log = false)
        {
            if (tags == null)
            {
                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                tags = tagManager.FindProperty("tags");
            }
            for (int i = 0; i < tags.arraySize; i++)
            {
                SerializedProperty t = tags.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(tag))
                {
                    if (log) Debug.Log("Tag already exists: " + tags);
                    return true;
                }
            }
            return false;
        }
    }
}
