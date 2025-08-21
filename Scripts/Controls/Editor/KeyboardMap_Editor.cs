
using UnityEditor;
using UnityEngine;

namespace TetraUtils
{
    [CustomEditor(typeof(KeyboardMap))]
    public class KeyboardMap_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            KeyboardMap keyboardMap = target as KeyboardMap;

            base.OnInspectorGUI();
        }
    }
}
