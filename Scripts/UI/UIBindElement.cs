#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TetraUtils
{
    public class UIBindElement : MonoBehaviour
    {
        public InputActionReference actionRef;
        public int bindingIndex;
        public InputBinding binding => actionRef.action.bindings[bindingIndex];
        public string actionName;
        [SerializeField]
        TextMeshProUGUI actionNameLabel;
        [SerializeField]
        TextMeshProUGUI bindingNameLabel;
        [SerializeField]
        Button actionButton;
        private void OnEnable()
        {
            actionButton.onClick.AddListener(Rebind);
        }

        private void OnDisable()
        {
            actionButton.onClick.RemoveListener(Rebind);
        }

        public void Rebind()
        {
            var action = actionRef.action;

            action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Gamepad") // optional
            .OnMatchWaitForAnother(0.1f)    // optional
            .OnComplete(operation =>
            {
                operation.Dispose();
                UpdateDisplay();
            })
            .Start();
        }

        public void UpdateDisplay()
        {
            actionRef.action.GetBindingDisplayString(bindingIndex, out string deviceName, out string controlPath);
            SetActionName(binding.isPartOfComposite ? actionRef.action.name + " " + binding.name : actionRef.action.name);
            SetBindName(controlPath);
        }
        public void SetActionName(string name)
        {
            actionNameLabel.text = name;
        }
        public void SetBindName(string name)
        {
            bindingNameLabel.text = name;
        }
    }
}
#endif
