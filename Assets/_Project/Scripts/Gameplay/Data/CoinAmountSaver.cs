using _Project.Scripts.ScriptableObjects.Bool;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Data
{
    public class CoinAmountSaver : MonoBehaviour
    {
        [SerializeField] private IntObjectSaver.IntData coinAmountData;
        [Space]
        
        [SerializeField] private BoolSo isWaveStartedBoolObjectSo;

        void Awake()
        {
            LoadCoinAmount();
        }

        public void LoadCoinAmount()
        {
            if (DataManager.Load(coinAmountData.dataKey, out int loadedValue))
            {
                coinAmountData.intObjectSo.SetValueTo(loadedValue);
            }
            else
            {
                coinAmountData.intObjectSo.ResetToInitialValue();
            }
        }

        void OnApplicationQuit()
        {
            if (isWaveStartedBoolObjectSo.Value)
            {
                return;
            }
            
            SaveCoinAmount();
        }

        public void SaveCoinAmount()
        {
            DataManager.Save(coinAmountData.dataKey, coinAmountData.intObjectSo.Value);
        }
    }
}
