using System;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.SoEventTransform
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(SoEventTransform))]
    public class SoEventTransform : ScriptableObject
    {
        public event Action<Transform> OnFire;

        public void RegisterToEvent(Action<Transform> method) => OnFire += method;
        public void DeregisterFromEvent(Action<Transform> method) => OnFire -= method;

        public void Invoke(Transform transform)
        {
            if (OnFire != null)
            {
                OnFire.Invoke(transform);
            }
        }
    }
}
