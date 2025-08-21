using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TetraUtils
{

    public class SceneLoader : MonoBehaviour
    {
        public GameObjectBinder binder;
        private void Awake()
        {
            SceneManager.sceneLoaded += SceneLoaded;
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
                binder = FindFirstObjectByType<GameObjectBinder>();
        }

        void SceneUnloaded(Scene scene)
        {
        }

        public void LoadWithLoadingScreen(string loadingSceneName, string sceneName)
        {
            SceneManager.LoadScene(loadingSceneName);
            var ls = FindAnyObjectByType<LoadingScreen>();
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            StartCoroutine(SceneLoadProgress(op, ls));
        }

        public IEnumerator SceneLoadProgress(AsyncOperation op, LoadingScreen ls)
        {
            op.allowSceneActivation = false;
            while (!op.isDone) 
            {
                ls.SetProgress(op.progress);
                yield return null;
            }

            op.allowSceneActivation = true;
        }
    }
}
