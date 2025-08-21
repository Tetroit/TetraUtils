using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TetraUtils
{
    public class Initialization : MonoBehaviour
    {
        [SerializeField] LoadingScreen loadingScreen;

        AsyncOperation op;

        private void Start()
        {
            op = SceneManager.LoadSceneAsync("SampleScene");
            StartCoroutine(Progress());
        }

        IEnumerator Progress()
        {
            op.allowSceneActivation = false;
            while (op.progress < 0.9f)
            {
                Debug.Log($"{op.progress}");
                loadingScreen.SetProgress(op.progress + 0.05f);
                yield return null;
            }

            op.allowSceneActivation = true;
            Debug.Log("Loading done!");
        }
    }
}
