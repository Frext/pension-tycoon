using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Building
{
    public class Room : MonoBehaviour
    {
        public enum RoomTypes
        {
            None,
            Room,
            DiningRoom,
            Wc,
            Reception
        }
        
        [Serializable]
        public class Slot
        {
            public bool isOccupied;

            [HideInInspector] public GameObject roomObject;
            
            public RoomTypes roomType;
        }

        public Slot slot;
        [Space]
        
        [Header("UI Slots")]
        [SerializeField] private GameObject roomSlotButton;
        [SerializeField] private GameObject wcSlotButton;
        [SerializeField] private GameObject diningRoomSlotButton;
        [SerializeField] private GameObject removeRoomWidth1Button;
        [SerializeField] private GameObject removeRoomWidth2Button;

        void Awake()
        {
            SetRoomSlotButton(false);
            SetWcSlotButton(false);
            SetDiningRoomSlotButton(false);
            SetRemoveRoomWidth1Button(false);
            SetRemoveRoomWidth2Button(false);
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
        
        public void SetRemoveRoomWidth1Button(bool activeState)
        {
            removeRoomWidth1Button.SetActive(activeState);
        }
        
        public void SetRemoveRoomWidth2Button(bool activeState)
        {
            removeRoomWidth2Button.SetActive(activeState);
        }

        #endregion
    }
}
