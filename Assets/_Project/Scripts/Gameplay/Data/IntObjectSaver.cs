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
            if (DataManager.Load(dataKey, out int loadedValue))
            {
                intObjectSo.SetValueTo(loadedValue);
            }
            else
            {
                intObjectSo.ResetToInitialValue();
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
