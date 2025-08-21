using System.Collections;
using System.Collections.Generic;
using TetraUtils;
using UnityEditor;
using UnityEngine;

namespace TetraUtils
{
    [CustomEditor(typeof(ModuleCollection))]
    public class ModuleCollectionEditor : Editor
    {
        string moduleName;
        public override void OnInspectorGUI()
        {
            var collection = (ModuleCollection)target;
            if (GUILayout.Button("Find Modules"))
            {
                collection.FindModules();
            }
            if (GUILayout.Button("Set Main"))
            {
                collection.SetMain();
            }

            if (GUILayout.Button("Generate Module Code"))
            {
                ScriptGenerator.GenerateScript("Assets/Scripts", "ModuleCollectionGenerated", collection.GenerateModulesCode());
            }
            base.OnInspectorGUI();
        }
    }

}
