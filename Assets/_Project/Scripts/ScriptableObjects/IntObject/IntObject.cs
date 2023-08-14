using System;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.SOEvent;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.ScriptableObjects.IntObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(IntObject))]
    public class IntObject : ScriptableObject
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

            foreach (var specialCondition in specialConditions)
            {
                if (Value == specialCondition.value)
                {
                    specialCondition.InvokeEvent();
                }
            }
        }
        
        public void IncrementValue(int amount = 1)
        {
            SetValueTo(Value + amount);
        }

        public void DecrementValue(int amount = 1)
        {
            SetValueTo(Value - amount);
        }
        
        public void IncrementValue(IntObject intObjectSo)
        {
            SetValueTo(Value + intObjectSo.Value);
        }
        
        public void DecrementValue(IntObject intObjectSo)
        {
            SetValueTo(Value - intObjectSo.Value);
        }

        public void ResetToInitialValue()
        {
            SetValueTo(initialValue);
        }
        
        public int GetMaxValue()
        {
            return max;
        }
    }
}
