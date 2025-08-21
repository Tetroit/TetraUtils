#if COLORFUL_HIERARCHY_INSTALLED
using Codice.Utils;
using MStudio;
#endif

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
#endif

using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Reflection;


namespace TetraUtils
{
    public class SetupEditor : EditorWindow
    {
        bool assemblyFoldout = false;
        bool definesFoldout = false;        

        Vector2 globalScroll;

        [MenuItem("Window/TetraUtils")]
        public static void ShowWindow()
        {
            GetWindow<SetupEditor>("TetraUtils");
        }
        private void OnGUI()
        {

            EditorGUILayout.LabelField("TetraUtils", Styles.HeaderStyle, GUILayout.Height(30));

            globalScroll = GUILayout.BeginScrollView(globalScroll);

            GUILayout.Label("Welcome to TetraUtils");
            if (GUILayout.Button("Locate Services"))
                LocationSetup();
#if TETRA_UTILS_INSTALLED

            if (GUILayout.Button("Install Script Templates"))
                SetScripts();

            if (GUILayout.Button("Setup"))
                StartSetup();
            assemblyFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(assemblyFoldout, "Assembly List");
            if (assemblyFoldout)
            {
                foreach (string s in Config.foundAssemblies)
                    GUILayout.Label(s);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            definesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(definesFoldout, "Defines List");
            if (definesFoldout)
            {
                foreach (var s in Config.defines)
                    GUILayout.Label(s);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.EndScrollView();
#endif
        }

        void SetScripts()
        {
            KeywordDefiner.RemoveScriptingDefineSymbol("TETRA_UTILS_SCRIPTS_INSTALLED");

            bool success = true;
            success &= ScriptGenerator.UnpackScript(Config.srcTemplate, "GameState", Config.dstTemplate, false);
            success &= ScriptGenerator.UnpackScript(Config.srcTemplate, "GameStateManager", Config.dstTemplate, false);

            AssetDatabase.Refresh();
            if (success)
            {
                KeywordDefiner.AddScriptingDefineSymbols("TETRA_UTILS_SCRIPTS_INSTALLED");
            }
        }
        public void StartSetup()
        {

#if COLORFUL_HIERARCHY_INSTALLED
            SetColorfulHierarchy();
#endif
            AllocateFolders();
            SetTags();
            SetScene();
            EditorUtility.SetDirty(this);
        }
        void AllocateFolders()
        {
            AssetLocator.CreateFolder("Assets", "Prefabs");
            AssetLocator.CreateFolder("Assets", "Textures");
            AssetLocator.CreateFolder("Assets", "Materials");
            AssetLocator.CreateFolder("Assets", "Shaders");
            AssetLocator.CreateFolder("Assets", "Scripts");
        }
        void SetTags()
        {
            TagUtils.AddTag("Binder");
            TagUtils.AddTag("GameManager");
            TagUtils.AddTag("SceneLoader");
        }
        void SetScene()
        {
            Scene current = SceneManager.GetActiveScene();

            GameObjectBinder binder = FindAnyObjectByType<GameObjectBinder>();
            if (binder == null)
            {
                GameObject go = new GameObject(GetColoredName("#r", "Binder"));
                go.tag = "Binder";
                binder = go.AddComponent<GameObjectBinder>();
            }

            CreateEnvironment(binder);
            CreateManagers(binder);
            CreateUI(binder);

            EditorSceneManager.MarkSceneDirty(current);
        }
        void CreateEnvironment(GameObjectBinder binder)
        {
            var prefab = AssetLocator.CreateEmptyPrefab("Assets/Prefabs/", "Environment");
            if (prefab == null)
            {
                Debug.Log("Failed to create environment prefab");
            }
            else if (!binder.FindBind("environment"))
            {
                var go = Instantiate(prefab);
                go.isStatic = true;
                go.name = GetColoredName("#o", "Environment");
                binder["environment"] = go;
            }
        }

        void CreateManagers(GameObjectBinder binder)
        {
#if TETRA_UTILS_SCRIPTS_INSTALLED
            var moduleCollection = FindAnyObjectByType<ModuleCollection>();
            if (moduleCollection == null)
            {
                var prefab = AssetLocator.GetPrefab("Assets/Prefabs/", "GameManager");
                if (prefab != null)
                {
                    var go = Instantiate(prefab);
                    go.name = GetColoredName("#y", "Modules");
                }
                else
                {
                    prefab = AssetLocator.CreateEmptyPrefab("Assets/Prefabs/", "Modules", (target) =>
                    {
                        var collection = target.AddComponent<ModuleCollection>();
                        collection.SetMain();
#if ENABLE_INPUT_SYSTEM
                        var userInputObj = new GameObject("User Input");
                        userInputObj.transform.SetParent(target.transform);
                        var userInput = userInputObj.AddComponent<UserInput>();
                        userInput.SetActionAsset(AssetLocator.FindAsset<InputActionAsset>(Config.inputActionsPath));
#else
                        Debug.Log("Install Input System package to get binding module!");
#endif
                        var sceneLoaderObj = new GameObject("Scene Loader");
                        sceneLoaderObj.transform.SetParent(target.transform);
                        var sceneLoader = sceneLoaderObj.AddComponent<SceneLoader>();

                        var gameStateManagerObj = new GameObject("Game State Manager");
                        gameStateManagerObj.transform.SetParent(target.transform);
                        var gameStateManager = gameStateManagerObj.AddComponent<GameStateManager>();

                        collection.FindModules();
                    });

                    prefab.isStatic = true;
                    prefab.tag = "GameManager";

                    var go = Instantiate(prefab);
                    go.name = GetColoredName("#y", "Modules");
                }
            }


#else
            Debug.LogWarning("Scripts were not installed, please install scripts first and try again to set up the components");
#endif
        }

        private void CreateUI(GameObjectBinder binder)
        {

#if TETRA_UTILS_SCRIPTS_INSTALLED
            var canvas = FindAnyObjectByType<Canvas>();
            if (canvas == null)
            {
                var canvasObj = new GameObject(GetColoredName("#c", "UI"));
                canvasObj.layer = LayerMask.NameToLayer("UI");

                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                var canvasScalerComp = canvasObj.AddComponent<CanvasScaler>();
                var graphicRaycasterComp = canvasObj.AddComponent<GraphicRaycaster>();
                canvasScalerComp.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScalerComp.referenceResolution = new Vector2(1920, 1080);
                var evSystemComp = FindAnyObjectByType<EventSystem>();
                if (evSystemComp == null)
                {
                    var evSystem = new GameObject("Event System");
                    evSystem.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
                    evSystem.AddComponent<InputSystemUIInputModule>();
#else
                    evSystem.AddComponent<StandaloneInputModule>();
#endif
                }
            }
            binder.AddBind("UI", canvas.gameObject);
#endif
        }

        string GetColoredName(string prefix, string name)
        {
#if COLORFUL_HIERARCHY_INSTALLED
            return prefix + name;
#else
            return name;
#endif
        }

        string GetUncoloredName(string name, string prefix)
        {
#if COLORFUL_HIERARCHY_INSTALLED
            if (name.StartsWith(prefix))
            {
                return name.Substring(prefix.Length);
            }
            else
            {
                Debug.Log($"Prefix {prefix} not recogninsed in {name}");
                return name;
            }
#else
            return name;
#endif
        }
        public void LocationSetup()
        {
            Config.defines.Clear();
            Config.ClearKeywords();
            LocateColorfulHierarchy();

            KeywordDefiner.AddScriptingDefineSymbols("TETRA_UTILS_INSTALLED");
            
            AssetDatabase.Refresh();
            Debug.Log("TetraUtils services located successfully.");
            EditorUtility.SetDirty(this);
        }
        
        void LocateColorfulHierarchy()
        {
            if (AssetDatabase.IsValidFolder("Assets/M Studio"))
            {
                KeywordDefiner.AddScriptingDefineSymbols("COLORFUL_HIERARCHY_INSTALLED");
            }

        }
#if COLORFUL_HIERARCHY_INSTALLED
        void SetColorfulHierarchy()
        {
            if (AssetDatabase.IsValidFolder("Assets/M Studio"))
            {
                Debug.Log("Colorful Hierarchy Package located");
            }
            else
            {
                Debug.Log("Package folder not found");
                return;
            }

            var palette = AssetDatabase.LoadAssetAtPath<ColorPalette>("Assets/M Studio/Colourful Hierarchy Category GameObject/Color Palette.asset");
            palette.colorDesigns.Clear();
            palette.colorDesigns.Add(DefaultDesignTemplate(PaletteFilter(1, 0, 0), "#r"));
            palette.colorDesigns.Add(DefaultDesignTemplate(PaletteFilter(1, 0.5f, 0), "#o"));
            palette.colorDesigns.Add(DefaultDesignTemplate(PaletteFilter(1, 1, 0), "#y"));
            palette.colorDesigns.Add(DefaultDesignTemplate(PaletteFilter(0.5f, 1, 0), "#l"));
            palette.colorDesigns.Add(DefaultDesignTemplate(PaletteFilter(0, 1, 0), "#g"));
            palette.colorDesigns.Add(DefaultDesignTemplate(PaletteFilter(0, 1, 1), "#c"));
            palette.colorDesigns.Add(DefaultDesignTemplate(PaletteFilter(0, 0, 1), "#b"));
            palette.colorDesigns.Add(DefaultDesignTemplate(PaletteFilter(0.5f, 0, 1), "#p"));
            palette.colorDesigns.Add(DefaultDesignTemplate(PaletteFilter(1, 0, 1), "#m"));
            palette.colorDesigns.Add(DefaultDesignTemplate(PaletteFilter(0, 0, 0), "#0"));

            EditorUtility.SetDirty(palette);
            Color PaletteFilter(float r, float g, float b)
            {
                return new Color(r / 2, g / 2, b / 2, 1);
            }
            ColorDesign DefaultDesignTemplate(Color col, string key)
            {
                return new ColorDesign()
                {

                    backgroundColor = col,
                    fontStyle = FontStyle.Bold,
                    textAlignment = TextAnchor.MiddleCenter,
                    textColor = Color.white,
                    keyChar = key
                };
            }

        }
#endif
    }
}
