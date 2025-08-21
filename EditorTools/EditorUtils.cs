using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace TetraUtils
{
    public static class EditorUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="playtimeAction"></param>
        /// <param name="editorTime"></param>
        public static T PerformEditorRuntimeDependant<T>(Func<T> playtimeAction, Func<T> editorTime)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                return playtimeAction();
            else
                return editorTime();
#else
            return playtimeAction();

#endif
        }
        public static void PerformEditorRuntimeDependant(Action playtimeAction, Action editorTime)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                playtimeAction?.Invoke();
            else
                editorTime?.Invoke();
#else
            playtimeAction?.Invoke();

#endif
        }

        public static void DestroyAllGameObjectsInArray<T>(ICollection<T> gameObjects) where T : Component
        {
            int amount = gameObjects.Count();
            for (int i = 0; i < amount; i++) 
            {
                var comp = gameObjects.ElementAt(i);
                if (comp == null) continue;
                var go = comp.gameObject;
                PerformEditorRuntimeDependant(  
                    () => UnityEngine.Object.Destroy(go),
                    () => UnityEngine.Object.DestroyImmediate(go));
            }
            gameObjects.Clear();
        }

        public static void DestroyAllChildrenOfType<T>(Component gameObject) where T : Component
        {
            var transform = gameObject.transform;
            int amount = transform.childCount;
            var coll = gameObject.GetComponentsInChildren<T>();
            foreach ( var c in coll )
            {
                PerformEditorRuntimeDependant(
                    () => UnityEngine.Object.Destroy(c.gameObject),
                    () => UnityEngine.Object.DestroyImmediate(c.gameObject));
            }
        }
    }
}
