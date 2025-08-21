using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TetraUtils
{
    public static class Styles
    {
        public static GUIStyle HeaderStyle { get; private set; }

        [InitializeOnLoadMethod]
        public static void Initialise()
        {
            HeaderStyle = new GUIStyle();
            HeaderStyle.fontSize = 24;
            HeaderStyle.fontStyle = FontStyle.Bold;
            HeaderStyle.normal.textColor = Color.white;
        }
    }
}
