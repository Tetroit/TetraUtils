using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TetraUtils
{
    public abstract class DeviceMap : ScriptableObject
    {
        public abstract List<(string, bool)> GetBoolEvents();
        public abstract List<(string, float)> GetFloatEvents();
    }
}