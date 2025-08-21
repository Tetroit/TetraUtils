using System;
using System.Collections;
using System.Collections.Generic;
using TetraUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TetraUtils
{
    public class UIBindsWindow : MonoBehaviour
    {
        List<UIBindElement> elements = new();

        [SerializeField]
        GameObject rebindPrefab;
        public void Read()
        {
            Clear();
            var asset = ModuleCollection.UserInput.GetActionAsset();
            string res = "";
            foreach (var map in asset.actionMaps) 
            {
                res += "MAP: " + map.name + "\n";
                foreach (var action in map.actions)
                {
                    res += "\tACTION: " + action.name + "\n";
                    for (int i = 0; i<action.bindings.Count; i++)
                    {
                        var bind = action.bindings[i];
                        res += "\t\tBIND: " + bind.name + "\n";
                        if (!bind.isComposite)
                        {

                            var element = Instantiate(rebindPrefab, transform);
                            var elementRebindComp = element.GetComponent<UIBindElement>();
                            elements.Add(elementRebindComp);
                            elementRebindComp.actionRef = InputActionReference.Create(action);
                            elementRebindComp.bindingIndex = i;
                            elementRebindComp.actionName = action.name;
                            elementRebindComp.UpdateDisplay();
                        }
                    }
                }
            }
            Debug.Log(res);
        }
        public void LoadFromPrefs()
        {

        }
        public void Clear()
        {
            EditorUtils.DestroyAllGameObjectsInArray(elements);
            EditorUtils.DestroyAllChildrenOfType<UIBindElement>(this);
        }
    }
}
