using System;
using Leguar.TotalJSON;
using UnityEngine;
using static _Project.Scripts.Gameplay.Building.Room.RoomTypeEnum;

namespace _Project.Scripts.Gameplay.Building
{
    public class Room : MonoBehaviour
    {
        [Serializable]
        public enum RoomTypeEnum
        {
            None,
            CustomerSingle,
            CustomerDouble,
            Dining,
            Bathroom,
            Arcade,
            Gym,
            Reception
        }
        
        public static int GetRoomWidth(RoomTypeEnum roomType)
        {
            return roomType is CustomerDouble or Dining ? 2 : 1;
        }
        
        [Serializable]
        public class RoomSlot
        {
            public bool isOccupied;
            public bool isUsable = true;
            public RoomTypeEnum roomType;
            
            [HideInInspector]
            [ExcludeFromJSONSerialize]
            public GameObject roomObject;

            public void ResetForSave()
            {
                isOccupied = false;
                isUsable = true;
            }
        }

        public RoomSlot slot;
        [Space]
        
        [Header("UI Slots")]
        [SerializeField] private GameObject removeRoomWidth1Button;
        [SerializeField] private GameObject removeRoomWidth2Button;
        [Space]
        [SerializeField] private GameObject makeRoomUsableWidth1Button;
        [SerializeField] private GameObject makeRoomUsableWidth2Button;
        
        #region Button Methods
        
        public void SetRemoveRoomWidth1Button(bool activeState)
        {
            removeRoomWidth1Button.SetActive(activeState);
        }
        
        public void SetRemoveRoomWidth2Button(bool activeState)
        {
            removeRoomWidth2Button.SetActive(activeState);
        }

        public void SetMakeUsableWidth1Button(bool activeState)
        {
            makeRoomUsableWidth1Button.SetActive(activeState);
        }
        
        public void SetMakeUsableWidth2Button(bool activeState)
        {
            makeRoomUsableWidth2Button.SetActive(activeState);
        }
        
        #endregion
    }
}
