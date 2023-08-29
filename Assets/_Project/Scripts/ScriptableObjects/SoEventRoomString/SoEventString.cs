using System;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.SoEventRoomString
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(SoEventString))]
    public class SoEventString : ScriptableObject
    {
        public event Action<string> OnFire;

        public void RegisterToEvent(Action<string>  method) => OnFire += method;
        public void DeregisterFromEvent(Action<string> method) => OnFire -= method;

        public void Invoke(string str)
        {
            if (OnFire != null)
            {
                OnFire.Invoke(str);
            }
        }
    }
}