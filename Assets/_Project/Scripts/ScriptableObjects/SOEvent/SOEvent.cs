using System;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.SOEvent
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(SoEvent))]
    public class SoEvent : ScriptableObject
    {
        public event Action OnFire;
        
        public void RegisterToEvent(Action method) => OnFire += method;
        public void DeregisterFromEvent(Action method) => OnFire -= method;

        public void Invoke()
        {
            if (OnFire != null)
            {
                OnFire.Invoke();
            }
        }
    }
}
