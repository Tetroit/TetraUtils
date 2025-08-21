using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TetraUtils
{
    public interface IProgressIndicator
    {
        public void Display(float state);
    }
    public class ProgressBar : MonoBehaviour, IProgressIndicator
    {
        public enum Mode
        {
            MASK = 0,
            WIDTH = 1,
        }

        public Image image;
        public Mode mode = Mode.WIDTH;

        [Range(0f, 1f)]
        public float progress;

        public float maxWidth;

        private void OnEnable()
        {
            image = GetComponent<Image>();
        }
        public void Display(float state)
        {
            progress = state;
            if (image != null)
            {
                switch (mode)
                {
                    case Mode.WIDTH:
                        image.rectTransform.sizeDelta = new Vector2(maxWidth * progress, image.rectTransform.sizeDelta.y);
                        break;
                    case Mode.MASK:
                        image.fillAmount = progress;
                        break;
                }
            }
        }
        void OnValidate()
        {
            Display(progress);
        }
    }
}

