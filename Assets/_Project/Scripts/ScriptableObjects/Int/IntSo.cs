using System;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.SOEvent;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.ScriptableObjects.Int
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(IntSo))]
    public class IntSo : ScriptableObject
    {
        [Serializable]
        private class ConditionalEvents
        {
            public UnityEvent OnReachValue;
            public int value;

            public void InvokeEvent()
            {
                OnReachValue.Invoke();
            }
        }
        
        [SerializeField] private SoEvent OnChangeValue;
        [SerializeField] private List<ConditionalEvents> specialConditions;
        [Space]
        
        [SerializeField] private int min = int.MinValue;
        [SerializeField] private int max = int.MaxValue;
        [Space] 
        
        [SerializeField] private int initialValue;
        
        public int Value { get; private set; }

        private bool isInitialized = false;

        void OnEnable()
        {
            SetValueTo(initialValue);
        }
        
        public void SetValueTo(int val)
        {
            Value = Mathf.Clamp(val, min, max);
            InvokeEvent();
        }
        
        private void InvokeEvent()
        {
            if (OnChangeValue != null)
            {
                OnChangeValue.Invoke();
            }

            if (isInitialized)
            {
                foreach (var specialCondition in specialConditions)
                {
                    if (Value == specialCondition.value)
                    {
                        specialCondition.InvokeEvent();
                    }
                }
            }
            else
            {
                isInitialized = true;
            }
        }
        
        public void SetValueTo(TextMeshProUGUI intText)
        {
            SetValueTo(int.Parse(intText.text));
        }
        
        public void IncrementValue(int amount = 1)
        {
            SetValueTo(Value + amount);
        }
        
        public void IncrementValue(IntSo intObjectSo)
        {
            SetValueTo(Value + intObjectSo.Value);
        }

        public void DecrementValue(int amount = 1)
        {
            SetValueTo(Value - amount);
        }
        
        public void DecrementValue(IntSo intObjectSo)
        {
            SetValueTo(Value - intObjectSo.Value);
        }
        
        public void DecrementValue(TextMeshProUGUI intText)
        {
            SetValueTo(Value - int.Parse(intText.text));
        }

        public void ResetToInitialValue()
        {
            SetValueTo(initialValue);
        }
        
        public float GetRatio()
        {
            return (float)Value / max;
        }

        public int GetMax()
        {
            return max;
        }
    }
}
