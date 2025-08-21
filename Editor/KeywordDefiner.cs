using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TetraUtils;
using UnityEditor;
using UnityEngine;

namespace TetraUtils
{
    public static class KeywordDefiner
    {
        public static void AddScriptingDefineSymbols(string define)
        {
            var current = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, $"{current};{define}");
            Config.defines.Add(define);
        }

        public static string[] GetScriptingDefineSymbols()
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(";");
        }

        public static void RemoveScriptingDefineSymbol(string symbol)
        {
            var words = GetScriptingDefineSymbols().ToList();
            for (int i = 0; i < words.Count; i++)
            {
                Debug.Log(i);
                if (words[i] == symbol)
                {
                    Config.defines?.Remove(words[i]);
                    words.RemoveAt(i);
                }
            }
            string res = string.Join(";", words);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, $"{res}");
        }

        public static void RemoveScriptingDefineSymbols(params string[] args)
        {
            var unfoundWords = args.ToList();
            var words = GetScriptingDefineSymbols().ToList();
            for (int i = 0; i < words.Count; i++)
            {
                for (int j = 0; j < unfoundWords.Count; j++)
                {
                    if (words[i] == unfoundWords[j])
                    {
                        Config.defines?.Remove(words[i]);
                        words.RemoveAt(i);
                        unfoundWords.RemoveAt(j);
                    }
                }

            }
            string res = string.Join(";", words);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, $"{res}");
        }
    }
}

