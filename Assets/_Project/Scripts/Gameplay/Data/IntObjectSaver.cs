using System;
using _Project.Scripts.ScriptableObjects.IntObject;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Data
{
    public class IntObjectSaver : MonoBehaviour
    {
        [SerializeField] private IntObject intObjectSo;
        [SerializeField] private string dataKey;

        void Awake()
        {
            LoadData();
        }

        private void LoadData()
        {
            int loadedValue = DataManager.Load<int>(dataKey);

            if (loadedValue == default)
            {
                intObjectSo.ResetToInitialValue();
            }
            else
            {
                intObjectSo.SetValueTo(loadedValue);
            }
        }

        void OnApplicationQuit()
        {
            SaveData();
        }
        
        private void SaveData()
        {
            DataManager.Save(dataKey, intObjectSo.Value);
        }
    }
}
