using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TetraUtils
{
    public interface IThrowBoolEvent
    {
        (bool, bool) ShouldThrowEvent();
    }
    public static class BindConfig
    {
        public static DeviceMap map;

        public static Dictionary<string, Action<bool>> booleanActions = new();
        public static void BindEvent(string action, Action<bool> function)
        {
            if (booleanActions.ContainsKey(action))
                booleanActions[action] += function;
            else
                booleanActions.Add(action, function);
        }

        public static void ProcessEvents()
        {
            foreach (var ev in map.GetBoolEvents())
            {
                booleanActions[ev.Item1]?.Invoke(ev.Item2);
            }
        }
    }
}