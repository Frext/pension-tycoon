using System;
using _Project.Scripts.Gameplay.Building;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects.SoEventRoomBool
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(SoEventRoomBool))]
    public class SoEventRoomBool : ScriptableObject
    {
        public event Func<Room, bool> OnFire;

        public void RegisterToEvent(Func<Room, bool> method) => OnFire += method;
        public void DeregisterFromEvent(Func<Room, bool> method) => OnFire -= method;

        public bool Invoke(Room room)
        {
            return OnFire != null && OnFire.Invoke(room);
        }
    }
}