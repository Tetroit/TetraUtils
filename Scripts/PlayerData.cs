using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TetraUtils
{

    public interface ISaved
    {
        void Save(PlayerData data);
        void Load(PlayerData data);
    }
    [Serializable]
    public class PlayerData
    {
        public int score;

        public void Save(string name)
        {
            string path = Path.Combine(Application.persistentDataPath, name) + ".sav";
            File.WriteAllText(path, JsonUtility.ToJson(this));
        }

        public static PlayerData Load(string name)
        {
            var path = Path.Combine(Application.persistentDataPath, name) + ".sav";
            if (File.Exists(name))
            {
                string input = File.ReadAllText(path);
                return JsonUtility.FromJson<PlayerData>(input);
            }
            return null;
        }
    }
}
