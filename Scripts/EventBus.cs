using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TetraUtils
{
    public static class EventBus
    {
        private static Dictionary<Type, List<Delegate>> m_Listeners = new();
        public static void Publish<T>(T ev) where T : Event
        {
            foreach (var listener in m_Listeners[typeof(T)])
            {
                ((Action<T>)listener)(ev);
            }
        }

        public static void Subscribe<T>(Action<T> handler) where T : Event
        {
            m_Listeners[typeof(T)].Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : Event
        {
            m_Listeners[typeof(T)]?.Remove(handler);
        }

        public abstract class Event
        {
        }
    }
}
