using Codice.Client.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TetraUtils
{
    public static class Config
    {
        public static readonly string[] definesList =
        {
            //packages & addons
            "COLORFUL_HIERARCHY_INSTALLED",
            //internal
            "TETRA_UTILS_INSTALLED",
            "TETRA_UTILS_SCRIPTS_INSTALLED",
        };

        public static string inputActionsPath => "Assets/TetraUtils/Scripts/Controls/Controls.inputactions";
        public static string dstTemplate => Path.Combine("Assets", "Scripts");
        public static string srcTemplate => Path.Combine("Assets", "TetraUtils", "ScriptTemplates");
        public static List<string> defines { get; private set; } = new();
        public static List<string> foundAssemblies { get; private set; } = new();

        [InitializeOnLoadMethod]
        public static void OnInit()
        {
            foundAssemblies = AssetLocator.ListAllAssemblies().ToList();
            DetectKeywords();
        }
        public static void ClearKeywords()
        {
            KeywordDefiner.RemoveScriptingDefineSymbols(definesList);
            defines.Clear();
        }
        public static void DetectKeywords()
        {
            defines.Clear();
            var keys = KeywordDefiner.GetScriptingDefineSymbols();
            foreach (var key in keys)
            {
                foreach (var match in definesList)
                {
                    if (key == match)
                        defines.Add(match);
                }
            }
        }
    }
}

