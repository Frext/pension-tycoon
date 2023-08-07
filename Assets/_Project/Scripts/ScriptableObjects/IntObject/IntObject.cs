using _Project.Scripts.ScriptableObjects.SOEvent;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.IntObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(IntObject))]
    public class IntObject : ScriptableObject
    {
        [SerializeField] private SoEvent OnChangeValue;
        
        public int Value { get; private set; }
        
        public void SetDayTo(int val)
        {
            Value = val;
            InvokeEvent();
        }
        
        private void InvokeEvent()
        {
            if (OnChangeValue != null)
            {
                OnChangeValue.Invoke();
            }
        }

        public void IncrementValue()
        {
            Value++;
            InvokeEvent();
        }
    }
}
