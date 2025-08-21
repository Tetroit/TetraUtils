using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UserInput))]
public class UserInputEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var userInput = (UserInput)target;
        if (GUILayout.Button("test"))
        {
            userInput.TestStuff();
        }
    }
}
