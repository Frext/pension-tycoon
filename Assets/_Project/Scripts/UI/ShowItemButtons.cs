using System;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.IntObject;
using _Project.Scripts.ScriptableObjects.SOEvent;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ShowItemButtons : MonoBehaviour
    {
        [Serializable]
        public class ScrollElement
        {
            public Button button;
            public int amount;
        }
        
        [SerializeField] private IntObject coinAmountSo;
        [SerializeField] private SoEvent OnChangeCoinAmount;
        [Space]
        
        [SerializeField] private List<ScrollElement> scrollElementsList;
        
        void Awake()
        {
            OnChangeCoinAmount.RegisterToEvent(UpdateButtonStates);
        }
        
        void UpdateButtonStates()
        {
            foreach (var scrollElement in scrollElementsList)
            {
                scrollElement.button.interactable = scrollElement.amount <= coinAmountSo.Value;
            }
        }

        void Start()
        {
            UpdateButtonStates();
        }
        
        void OnDestroy()
        {
            OnChangeCoinAmount.DeregisterFromEvent(UpdateButtonStates);
        }
    }
}
