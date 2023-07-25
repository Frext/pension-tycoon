using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class Room : MonoBehaviour
    {
        public enum RoomTypes
        {
            Room,
            DiningRoom,
            Wc,
            Reception
        }
        
        [Serializable]
        public class RoomSlot
        {
            public bool isOccupied;
            
            public RoomTypes roomType;
        }

        public RoomSlot roomSlot;
        [Space]
        
        [Header("UI Slots")]
        [SerializeField] private GameObject roomSlotButton;
        [SerializeField] private GameObject wcSlotButton;
        [SerializeField] private GameObject diningRoomSlotButton;

        void Awake()
        {
            SetRoomSlotButton(false);
            SetWcSlotButton(false);
            SetDiningRoomSlotButton(false);
        }

        #region RoomManager methods

        public void SetRoomSlotButton(bool activeState)
        {
            roomSlotButton.SetActive(activeState);
        }

        public void SetWcSlotButton(bool activeState)
        {
            wcSlotButton.SetActive(activeState);
        }
        
        public void SetDiningRoomSlotButton(bool activeState)
        {
            diningRoomSlotButton.SetActive(activeState);
        }

        public bool IsDiningRoomSlotButtonActive()
        {
            return diningRoomSlotButton.activeInHierarchy;
        }

        #endregion
    }
}
