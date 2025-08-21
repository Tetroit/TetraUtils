using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TetraUtils
{
    public class GameObjectBinder : MonoBehaviour
    {
        public static GameObjectBinder main;

        [Serializable]
        public struct Bind
        {
            public string key;
            public GameObject obj;
            public Bind(string key, GameObject obj)
            {
                this.key = key;
                this.obj = obj;
            }
        }

        public List<Bind> binds = new List<Bind>();


        private void Awake()
        {
            if (main == null)
               main = this;
            else
                Destroy(this);
        }
        public GameObject FindBind(string key)
        {
            foreach (Bind bind in binds)
            {
                if (bind.key.Equals(key))
                    return bind.obj;
            }
            return null;
        }
        public void AddBind(string key, GameObject obj)
        {
            if (FindBind(key) == null)
            {
                Debug.Log($"Bind added: {key}");
                binds.Add(new Bind(key, obj));
            }
            else
            {
                Debug.Log($"{key} bind already exists");
            }
        }
        public void Remove(string key)
        {
            binds.Remove(binds.Find((Bind b) => b.key == key));
        }

        public GameObject this[string key]
        {
            get => FindBind(key);
            set => AddBind(key, value);
        }
    }
}
