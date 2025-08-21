using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TetraUtils
{
    public static class AssetLocator
    {
        public static void CreateFolder(string path, string name)
        {
            if (!HasFolder(path))
            {
                Debug.Log($"{path} doesn't exist.");
                return;
            }
            if (HasFolder($"{path}/{name}"))
            {
                Debug.Log($"{name} folder already exists.");
                return;
            }
            AssetDatabase.CreateFolder(path, name);
        }
        public static bool HasFolder(string path, bool log = false)
        {
            if (!log)
                return AssetDatabase.IsValidFolder(path);
            bool val = AssetDatabase.IsValidFolder(path);
            if (!val)
                Debug.Log($"Invalid path: {path}.");
            return val;
        }

        public static GameObject CreatePrefab(GameObject go, string path, string name, Action<GameObject> onCreate = null)
        {
            if (!HasFolder(path, true)) return null;
            GameObject prefab = GetPrefab(path, name);
            if (prefab == null)
            {
                if (onCreate != null)
                    onCreate(go);
                return PrefabUtility.SaveAsPrefabAsset(go, $"{path}/{name}.prefab");
            }
            else
            {
                return prefab;
            }
        }

        public static GameObject CreateEmptyPrefab(string path, string name, Action<GameObject> onCreate = null)
        {
            if (!HasFolder(path, true)) return null;
            GameObject prefab = GetPrefab(path, name);
            if (prefab == null)
            {
                var go = new GameObject(name);
                if (onCreate != null)
                    onCreate(go);
                prefab = PrefabUtility.SaveAsPrefabAsset(go, $"{path}/{name}.prefab");
                UnityEngine.Object.DestroyImmediate(go);
            }
            return prefab;
        }

        public static bool HasPrefab(string path, string name)
        {
            if (!HasFolder(path, true)) return false;
            return AssetDatabase.LoadAssetAtPath<GameObject>($"{path}/{name}.prefab") == null;
        }
        public static GameObject GetPrefab(string path, string name)
        {
            if (!HasFolder(path, true)) return null;
            return AssetDatabase.LoadAssetAtPath<GameObject>($"{path}/{name}.prefab");
        }
        public static bool HasAsset(string path)
        {
            if (!HasFolder(path, true)) return false;

            var matches = AssetDatabase.FindAssets(path);
            if (matches.Length == 0)
                return false;
            return true;
        }

        public static T FindAsset<T>(string path)
            where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }


        public static string[] ListAllAssemblies()
        {
            List<string> res = new();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                res.Add($"Assembly: {assembly.GetName().Name}");
            }
            return res.ToArray();
        }

        public static Assembly GetAssembly(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                if (assembly.GetName().Name == name)
                    return assembly;
            }
            return null;
        }
    }
}
