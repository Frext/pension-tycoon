using System;
using _Project.Scripts.ScriptableObjects.Int;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static _Project.Scripts.ScriptableObjects.FloatRange.FloatRangeSo;

namespace _Project.Scripts.UI.Shop
{
    public class ShowShopItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnTurnOn;
        [SerializeField] private UnityEvent OnTurnOff;
        [Space] 
        [SerializeField] private TextMeshProUGUI showAfterLevelTextMesh;
        [SerializeField] private bool shouldAppendNumberToText = true;
        
        [SerializeField] private IntSo dayCountSo;
        
        [Tooltip("Inclusive")] 
        [SerializeField] private FloatRange showBetweenLevels;

        void Start()
        {
            if (shouldAppendNumberToText)
            {
                showAfterLevelTextMesh.text += showBetweenLevels.min;
            }
        }

        void OnEnable()
        {
            UpdateVisibility();
        }
        
        public void UpdateVisibility()
        {
            if (showBetweenLevels.IsBetween(dayCountSo.Value))
            {
                OnTurnOn.Invoke();
            } else {
                OnTurnOff.Invoke();
            }
        }
    }
}
