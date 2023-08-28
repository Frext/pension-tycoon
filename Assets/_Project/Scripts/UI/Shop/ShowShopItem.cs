using _Project.Scripts.ScriptableObjects.Int;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.UI.Shop
{
    public class ShowShopItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnTurnOn;
        [SerializeField] private UnityEvent OnTurnOff;
        [Space] 
        [SerializeField] private TextMeshProUGUI textMesh;
        
        [SerializeField] private IntSo dayCountSo;
        
        [Tooltip("Inclusive")] [Range(0, 20)] [SerializeField] private int showAfterLevel;
        
        void Start()
        {
            UpdateVisibility();

            textMesh.text += showAfterLevel.ToString();
        }
        
        public void UpdateVisibility()
        {
            if (dayCountSo.Value >= showAfterLevel)
            {
                OnTurnOn.Invoke();
            } else {
                OnTurnOff.Invoke();
            }
        }
    }
}
