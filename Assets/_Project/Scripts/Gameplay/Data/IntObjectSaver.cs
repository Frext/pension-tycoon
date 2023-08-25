using System;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.IntObject;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Data
{
    public class IntObjectSaver : MonoBehaviour
    {
        [Serializable]
        public class IntData
        {
            public IntObject intObjectSo;
            public string dataKey;
        }
        
        [SerializeField] private List<IntData> intDataList;

        void Awake()
        {
            LoadData();
        }

        private void LoadData()
        {
            foreach (var intData in intDataList)
            {
                if (DataManager.Load(intData.dataKey, out int loadedValue))
                {
                    intData.intObjectSo.SetValueTo(loadedValue);
                }
                else
                {
                    intData.intObjectSo.ResetToInitialValue();
                }
            }
        }

        void OnApplicationQuit()
        {
            SaveData();
        }
        
        private void SaveData()
        {
            foreach (var intData in intDataList)
            {
                DataManager.Save(intData.dataKey, intData.intObjectSo.Value);
            }
        }
    }
}
