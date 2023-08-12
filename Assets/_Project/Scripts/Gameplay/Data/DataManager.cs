using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Leguar.TotalJSON;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Data
{
    public class DataManager : MonoBehaviour
    {
        private static string filePath => Application.persistentDataPath + "/";
        
        public static void Save(string key, object value)
        {
            JSON jsonObject = new JSON();
            
            jsonObject.Add(key, SerializeByType(value));

            string stringRepresentation = jsonObject.CreateString();
            
            File.WriteAllText(filePath + key, stringRepresentation);
        }

        static object SerializeByType(object value)
        {
            Type tType = value.GetType();

            if (tType.IsPrimitive)
            {
                return value;
            }
            if (value is IList || value is Array)
            {
                return JArray.Serialize(value);
            }
            if (value is IDictionary)
            {
                return JSON.Serialize(value);
            }
            if (tType.IsSerializable)
            {
                return JSON.Serialize(value);
            }

            return new JNull();
        }

        public static T Load<T>(string key)
        {
            if (!File.Exists(filePath + key))
            {
                return default;
            }
            
            JSON jsonObject = JSON.ParseString(File.ReadAllText(filePath + key));
            
            jsonObject.DebugInEditor(key);

            return !jsonObject.ContainsKey(key) ? default : DeserializeByType<T>(jsonObject, key);
        }

        private static T DeserializeByType<T>(JSON jsonObject, string key)
        {
            Type tType = typeof(T);
            
            if (tType == typeof(int))
            {
                return (T)(object)jsonObject.GetInt(key);
            }
            if (tType == typeof(float))
            {
                return (T)(object)jsonObject.GetFloat(key);
            }
            if (tType == typeof(bool))
            {
                return (T)(object)jsonObject.GetBool(key);
            }
            if (tType == typeof(string))
            {
                return (T)(object)jsonObject.GetString(key);
            }
            if (tType == typeof(IList) || tType == typeof(Array))
            {
                return (T)(object)jsonObject.GetJArray(key).AsList();
            }
            if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return (T)jsonObject.Get(key).zDeserialize(tType, null, new DeserializeSettings());
            }
            if (tType.IsSerializable)
            {
                return (T)jsonObject.Get(key).zDeserialize(tType, null, new DeserializeSettings());
            }
            
            throw new Exception("Unsupported data type '" + tType + "'.");
        }
    }
}
