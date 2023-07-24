using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class Room : MonoBehaviour
    {
        public enum RoomTypes
        {
            None,
            Room,
            DiningRoom,
            Wc
        }
        
        [Serializable]
        public class RoomSlot
        {
            public bool isOccupied;
            
            public RoomTypes roomType;
        }

        public RoomSlot roomSlot;
        [Space]
        
        [SerializeField] private GameObject width1Slot;
        [SerializeField] private GameObject width2Slot;

        void Awake()
        {
            SetSlotWidth1(false);
            SetSlotWidth2(false);
        }

        #region RoomManager methods

        public void SetSlotWidth1(bool activeState)
        {
            width1Slot.SetActive(activeState);
        }

        public void SetSlotWidth2(bool activeState)
        {
            width2Slot.SetActive(activeState);
        }

        public bool IsSlotWidth2Active()
        {
            return width2Slot.activeInHierarchy;
        }

        #endregion
    }
}
