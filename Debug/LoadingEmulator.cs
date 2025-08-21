using System;
using System.Collections;
using System.Collections.Generic;
using TetraUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TetraUtils
{
    public class LoadingEmulator : MonoBehaviour
    {
        [SerializeField]
        LoadingScreen loadingScreen;
        public Action onLoadingFinish;

        private void Start()
        {
            SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
            StartCoroutine(Progress());
        }
        IEnumerator Progress()
        {
            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime * 0.1f;
                loadingScreen.SetProgress(progress);
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("Loading done!");
        }
    }
}
