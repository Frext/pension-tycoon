using _Project.Scripts.ScriptableObjects.IntObject;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.UI
{
    public class ShowScrollItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnTurnOn;
        [SerializeField] private UnityEvent OnTurnOff;
        [Space] 
        [SerializeField] private TextMeshProUGUI textMesh;
        
        [SerializeField] private IntObject dayCountSo;
        
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
