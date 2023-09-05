using System;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.SoEventVector3
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(SoEventVector3))]
    public class SoEventVector3 : ScriptableObject
    {
        public event Action<Vector3> OnFire;
        
        public void RegisterToEvent(Action<Vector3> method) => OnFire += method;
        public void DeregisterFromEvent(Action<Vector3> method) => OnFire -= method;

        public void Invoke(Vector3 pos)
        {
            if (OnFire != null)
            {
                OnFire.Invoke(pos);
            }
        }
    }
}