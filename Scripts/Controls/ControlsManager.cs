using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TetraUtils
{
    public class ControlsManager : MonoBehaviour
    {
        [SerializeField]
        KeyboardMap keyboardMap;
        private void Start()
        {
            BindConfig.map = keyboardMap;
            keyboardMap.AddBoolBind("Interact", KeyCode.E);
            BindConfig.BindEvent("Attack", Attack);
        }
        void Attack(bool value)
        {
            if (value)
                Debug.Log("You attack");
        }

        private void Update()
        {
            BindConfig.ProcessEvents();
        }
    }
}
