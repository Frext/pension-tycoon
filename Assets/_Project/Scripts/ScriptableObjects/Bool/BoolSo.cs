using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.Bool
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(BoolSo))]
    public class BoolSo : ScriptableObject
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
