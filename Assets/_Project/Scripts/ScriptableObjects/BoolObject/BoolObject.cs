using System;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.BoolObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(BoolObject))]
    public class BoolObject : ScriptableObject
    {
        public bool Value { get; private set; }

        [SerializeField] private bool initialValue;

        void OnEnable()
        {
            SetTo(initialValue);
        }

        public void SetTo(bool state)
        {
            Value = state;
        }
    }
}
