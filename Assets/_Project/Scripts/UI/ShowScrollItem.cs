using _Project.Scripts.ScriptableObjects.IntObject;
using _Project.Scripts.ScriptableObjects.SOEvent;
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
        
        [SerializeField] private SoEvent OnChangeDay;
        [SerializeField] private IntObject dayCountSo;
        
        [Tooltip("Inclusive")] [Range(0, 100)] [SerializeField] private int showAfterLevel;

        void Awake()
        {
            OnChangeDay.RegisterToEvent(UpdateVisibility);
        }
        
        private void UpdateVisibility()
        {
            if (dayCountSo.Value >= showAfterLevel)
            {
                OnTurnOn.Invoke();
            } else {
                OnTurnOff.Invoke();
            }
        }

        void Start()
        {
            UpdateVisibility();

            textMesh.text += showAfterLevel.ToString();
        }
        
        private void OnDestroy()
        {
            OnChangeDay.DeregisterFromEvent(UpdateVisibility);
        }
    }
}
