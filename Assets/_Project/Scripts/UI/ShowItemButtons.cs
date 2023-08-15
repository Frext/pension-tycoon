using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.IntObject;
using _Project.Scripts.ScriptableObjects.SOEvent;
using TMPro;
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
            public TextMeshProUGUI priceText;
        }

        [Tooltip("Update delay is needed for the update buttons to change their text before starting to update buttons.")]
        [SerializeField] private float updateDelay = 0.001f;
        [Space]
        
        [SerializeField] private IntObject coinAmountSo;
        [SerializeField] private SoEvent OnChangeCoinAmount;
        [Space]
        
        [SerializeField] private List<ScrollElement> scrollElementsList;
        [Space] 
        [SerializeField] private GameObject panelObject;
        
        void Awake()
        {
            OnChangeCoinAmount.RegisterToEvent(UpdateButtonStatesAfterDelay);
        }
        
        void UpdateButtonStatesAfterDelay()
        {
            if (panelObject.activeInHierarchy)
            {
                StartCoroutine(IUpdateButtons(updateDelay));
            }
        }
        
        private IEnumerator IUpdateButtons(float time = 0f)
        {
            yield return new WaitForSeconds(time);
            
            foreach (var scrollElement in scrollElementsList)
            {
                // If it's a number, enable or disable the button.
                if (int.TryParse(scrollElement.priceText.text, out int amount))
                {
                    scrollElement.button.interactable = amount <= coinAmountSo.Value;
                }
            }
        }
        
        void Start()
        {
            StartCoroutine(IUpdateButtons());
        }

        void OnDestroy()
        {
            OnChangeCoinAmount.DeregisterFromEvent(UpdateButtonStatesAfterDelay);
        }
    }
}
