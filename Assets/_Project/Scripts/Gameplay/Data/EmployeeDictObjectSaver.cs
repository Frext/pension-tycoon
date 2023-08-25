using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.EmployeeDictObject;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Data
{
    public class EmployeeDictObjectSaver : MonoBehaviour
    {
        public EmployeeDictObject employeeCountDictSo;
        public string dataKey;

        void Awake()
        {
            LoadData();
        }

        private void LoadData()
        {
            employeeCountDictSo.SetToDefault();

            // Change the values if the deserialized dictionary is loaded.
            if (DataManager.Load(dataKey, out Dictionary<string, int> deserialized))
            {
                employeeCountDictSo.LoadFromSerializableDictionary(deserialized);
            }
        }
        
        void OnApplicationQuit()
        {
            SaveData();
        }
        
        private void SaveData()
        {
            DataManager.Save(dataKey, employeeCountDictSo.ToSerializableDictionary());
        }
    }
}
