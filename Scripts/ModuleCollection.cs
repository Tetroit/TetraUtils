using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TetraUtils
{
    /// <summary>
    /// A class for storing all singleton runtime persistent references, 
    /// useful for global systems, such as game manager, score manager. 
    /// Add it as a separate child object and it will automatically be DontDestroyOnLoad at program start.
    /// </summary>
    [ExecuteAlways]
    public partial class ModuleCollection : MonoBehaviour
    {
        [SerializeField]
        List<string> modules;
        static ModuleCollection instance;
        public static ModuleCollection Instance 
        { 
        get {
                if (instance != null) return instance;
                instance = FindAnyObjectByType<ModuleCollection>();
                if (instance != null) return instance;
                Debug.LogWarning("No instance of ModuleCollection found, returning null");
                return null;
            }
        }
        private void Awake()
        {
            if (Application.isPlaying)
            {
                if (Instance == null)
                    instance = this;
                else if (Instance != this)
                {
                    Destroy(Instance);
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                instance = this;
            }
        }
        private void OnDisable()
        {
            if (!Application.isPlaying)
            {
                instance = null;
            }
        }
        public void SetMain()
        {
            instance = this;
        }

        public string GenerateModulesCode()
        {
            string ToLowercase(string name)
            {
                var cap = name.Substring(0, 1);
                cap = cap.ToLower();
                return cap + name.Substring(1, name.Length - 1);
            }
            string ToUppercase(string name)
            {
                var cap = name.Substring(0, 1);
                cap = cap.ToUpper();
                return cap + name.Substring(1, name.Length - 1);
            }
            string GenerateProperty(string module)
            {
                return $"        [SerializeField]\r\n" +
                    $"        {module} {ToLowercase(module)};\r\n";
            }
            string GenerateStaticReference(string module)
            {
                return $"\r\n" +
                    $"        public static {module} {ToUppercase(module)} => Instance.{ToLowercase(module)};\r\n";
            }
            string GenerateFinder(string module)
            {
                return $"\r\n" +
                    $"            {ToLowercase(module)} = GetComponentInChildren<{module}>();\r\n" +
                    $"            if ({ToLowercase(module)} == null) Debug.Log(\"{ToUppercase(module)} not found\");\r\n";
            }
            string code = "using UnityEngine;\r\n\r\n" +
                "namespace TetraUtils\r\n{\r\n" +
                "    public partial class ModuleCollection : MonoBehaviour\r\n" +
                "    {\r\n";
            foreach(var module in modules)
            {
                code += GenerateProperty(module);
            }
            code += "\r\n";
            foreach (var module in modules)
            {
                code += GenerateStaticReference(module);
            }
            code += "\r\n" +
                "        public void FindModules()\r\n" +
                "        {\r\n";
            foreach (var module in modules)
            {
                code += GenerateFinder(module);
            }
            code += "        }\r\n" +
                "        public static void FindModulesStatic()\r\n" +
                "        {\r\n" +
                "            Instance.FindModules();\r\n" +
                "        }\r\n" +
                "    }\r\n" +
                "}";
            return code;
        }
    }

}