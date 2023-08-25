using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.NPC;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.EmployeeDictObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(EmployeeDictObject))]
    public class EmployeeDictObject : ScriptableObject
    {
        public enum EmployeeTypesEnum
        {
            Cook,
            Cleaner,
            GameTechnician,
            GymCoach
        }
        
        public readonly Dictionary<EmployeeTypesEnum, int> employeeCountDict = new();
        public readonly Dictionary<EmployeeTypesEnum, List<Employee>> employeeScriptsDict = new();

        public void SetToDefault()
        {
            SetToDefaultEmployeeCountDict();
            SetToDefaultEmployeeScriptsDict();
        }

        private void SetToDefaultEmployeeCountDict()
        {
            employeeCountDict.Clear();
            
            // Add each type in enum to the dictionary before changing values.
            for (int index = 0; index < Enum.GetNames(typeof(EmployeeTypesEnum)).Length; index++)
            {
                employeeCountDict.Add((EmployeeTypesEnum)index, 0);
            }
        }
        
        private void SetToDefaultEmployeeScriptsDict()
        {
            employeeScriptsDict.Clear();
            
            // Add each type in enum to the dictionary before changing values.
            for (int index = 0; index < Enum.GetNames(typeof(EmployeeTypesEnum)).Length; index++)
            {
                employeeScriptsDict.Add((EmployeeTypesEnum)index, new List<Employee>());
            }
        }

        public void LoadFromSerializableDictionary(Dictionary<string, int> deserializedDict)
        {
            foreach (var keyValuePair in deserializedDict)
            {
                if (Enum.TryParse(keyValuePair.Key, out EmployeeTypesEnum keyEnum))
                {
                    employeeCountDict[keyEnum] = keyValuePair.Value;
                }
            }
        }

        public Dictionary<string, int> ToSerializableDictionary()
        {
            Dictionary<string, int> serializedDict = new Dictionary<string, int>();

            foreach (var keyValuePair in employeeCountDict)
            {
                // Convert the enum key to string
                serializedDict.Add(keyValuePair.Key.ToString(), keyValuePair.Value);
            }

            return serializedDict;
        }
    }
}
