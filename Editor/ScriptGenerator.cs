
using System.IO;
using UnityEngine;
using UnityEditor;

namespace TetraUtils
{
    public static class ScriptGenerator
    {
        public static bool UnpackScript(string srcPath, string srcName, string dstPath, bool refresh = true)
            => UnpackScript(srcPath, srcName, dstPath, srcName, refresh);
        public static bool UnpackScript(string srcPath, string srcName, string dstPath, string dstName, bool refresh = true)
        {
            string fullSrcPath = Path.Combine(srcPath, srcName + ".cs.txt");
            if (!File.Exists(fullSrcPath))
            {
                Debug.LogError($"No file at path: {fullSrcPath}");
                return false;
            }
            if (!Directory.Exists(dstPath))
            {
                Debug.LogWarning($"No dst directory exists at {dstPath}, creating a new one");
                Directory.CreateDirectory(dstPath);
            }
            string s = File.ReadAllText(fullSrcPath);
            GenerateScript(dstPath, dstName, s, refresh);
            if (refresh)
                AssetDatabase.Refresh();

            return true;
        }
        public static void GenerateScript(string path, string name, string code, bool refresh = true)
        {
            string fullPath = Path.Combine(path, name + ".cs");

            File.WriteAllText(fullPath, code);

            if (refresh)
                AssetDatabase.Refresh();
        }
    }
}
