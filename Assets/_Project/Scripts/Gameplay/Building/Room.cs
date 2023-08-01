using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Building
{
    public class Room : MonoBehaviour
    {
        public enum RoomTypeEnum
        {
            None,
            CustomerSingle,
            CustomerDouble,
            Dining,
            Wc,
            Reception
        }
        
        public static int GetRoomWidth(RoomTypeEnum roomType)
        {
            return roomType is RoomTypeEnum.CustomerDouble or RoomTypeEnum.Dining ? 2 : 1;
        }
        
        [Serializable]
        public class Slot
        {
            public bool isOccupied;
            public RoomTypeEnum roomType;
            
            [HideInInspector] public GameObject roomObject;
        }

        public Slot slot;
        [Space]
        
        [Header("UI Slots")]
        [SerializeField] private GameObject removeRoomWidth1Button;
        [SerializeField] private GameObject removeRoomWidth2Button;

        
        void Awake()
        {
            SetRemoveRoomWidth1Button(false);
            SetRemoveRoomWidth2Button(false);
        }

        #region Button Methods
        
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
