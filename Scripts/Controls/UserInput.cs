using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public struct BindingInfo
    {
        public string name; 
        public string bindPath;
        public string secondaryBindPath;
    }
    [SerializeField]
    InputActionAsset actionAsset;
    public void SetActionAsset(InputActionAsset actionAsset)
    {
        this.actionAsset = actionAsset;
    }

    public void TestStuff()
    {
        var action = actionAsset.FindAction("Interact");
        action.ChangeBinding(0).WithPath("<Keyboard>/R");
        Debug.Log("Changing shit");
    }

    public InputActionAsset GetActionAsset() => actionAsset;
}
