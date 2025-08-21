using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TetraUtils
{
    [CustomEditor(typeof(UIBindsWindow))]
    public class UIBindsWindowEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var uibinder = target as UIBindsWindow;

            if (GUILayout.Button("Read Input Binds"))
            {
                uibinder.Read();
            }
            base.OnInspectorGUI();
        }
    }
}
