using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TetraUtils
{
   public class LoadingScreen : MonoBehaviour
    {
        [SerializeField]
        public IProgressIndicator progressIndicator;
        void OnEnable()
        {
            progressIndicator = GetComponentInChildren<IProgressIndicator>();
        }
        public void SetProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            progressIndicator.Display(progress);
        }
    }
}