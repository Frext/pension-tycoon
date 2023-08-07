using System;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.SoEventGameObject
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(SoEventGameObject))]
    public class SoEventGameObject : ScriptableObject
    {
        public event Action<GameObject> OnFire;
        
        public void RegisterToEvent(Action<GameObject> method) => OnFire += method;
        public void DeregisterFromEvent(Action<GameObject> method) => OnFire -= method;

        public void Invoke(GameObject gameObject)
        {
            if (OnFire != null)
            {
                OnFire.Invoke(gameObject);
            }
        }
    }
}
