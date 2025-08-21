using System;
using System.Collections;
using System.Collections.Generic;
using TetraUtils;
using UnityEngine;

namespace TetraUtils
{
    [CreateAssetMenu(fileName = "Keyboard Map", menuName = "TetraUtils/Controls/Keyboard Map", order = 0)]
    public class KeyboardMap : DeviceMap
    {
        [Serializable]
        public struct BooleanBind : IThrowBoolEvent
        {
            public string name;
            public KeyCode[] binds;
            public BooleanBind(string name, KeyCode bind1)
            {
                this.name = name;
                binds = new KeyCode[] { bind1 };
            }
            public BooleanBind(string name, KeyCode bind1, KeyCode bind2)
            {
                this.name = name;
                binds = new KeyCode[] { bind1, bind2 };
            }
            public (bool, bool) ShouldThrowEvent()
            {
                foreach (KeyCode kc in binds)
                {
                    if (Input.GetKeyDown(kc))
                        return (true, true);
                    if (Input.GetKeyUp(kc))
                        return (true, false);
                }
                return (false, false);
            }
        }
        [SerializeField]
        List<BooleanBind> booleanActions = new();

        public void AddBoolBind(string name = "New bind", KeyCode code = KeyCode.None)
        {
            booleanActions.Add(new BooleanBind(name, code));
        }

        public override List<(string, bool)> GetBoolEvents()
        {
            List<(string, bool)> res = new();
            foreach (var bind in booleanActions)
            {
                var info = bind.ShouldThrowEvent();
                if (info.Item1)
                    res.Add((bind.name, info.Item2));
            }
            return res;
        }

        public List <BooleanBind> BooleanBinds { get => new (booleanActions); set => booleanActions = value; }
        public override List<(string, float)> GetFloatEvents()
        {
            throw new NotImplementedException();
        }
    }
}