using System;
using _Project.Scripts.Gameplay.Building;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.SoEventTransform
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(SoEventRoom))]
    public class SoEventRoom : ScriptableObject
    {
        public event Action<Room> OnFire;

        public void RegisterToEvent(Action<Room> method) => OnFire += method;
        public void DeregisterFromEvent(Action<Room> method) => OnFire -= method;

        public void Invoke(Room room)
        {
            if (OnFire != null)
            {
                OnFire.Invoke(room);
            }
        }
    }
}
