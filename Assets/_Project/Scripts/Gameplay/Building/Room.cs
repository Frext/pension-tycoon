using System;
using System.Collections.Generic;
using Leguar.TotalJSON;
using UnityEngine;
using static _Project.Scripts.Gameplay.Building.Room.RoomTypeEnum;
using static _Project.Scripts.ScriptableObjects.EmployeeDictObject.EmployeeDictObject;

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
            return roomType is CustomerDouble ? 2 : 1;
        }
        
        public static EmployeeTypesEnum GetEmployeeTypeForRoom(RoomTypeEnum roomType)
        {
            return roomType switch
            {
                Bathroom => EmployeeTypesEnum.Cleaner,
                CustomerSingle => EmployeeTypesEnum.Cleaner,
                CustomerDouble => EmployeeTypesEnum.Cleaner,
                Dining => EmployeeTypesEnum.Cook,
                Arcade => EmployeeTypesEnum.GameTechnician,
                Gym => EmployeeTypesEnum.GymCoach,
                _ => throw new ArgumentOutOfRangeException(nameof(roomType), roomType, null)
            };
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

        
        [Serializable]
        public class RoomButton
        {
            public GameObject roomButton;
            public RectTransform roomButtonRectTransform;
            public List<RectTransform> roomButtonPositionsList;
        }

        [Header("UI Slots")] 
        [SerializeField] private RoomButton removeRoomButton;
        [SerializeField] private RoomButton makeRoomUsableButton;
        

        #region Button Methods
        
        public void SetRemoveRoomButton(bool activeState, int width = 1)
        {
            SetButton(removeRoomButton, activeState, width);
        }
        
        public void SetMakeUsableButton(bool activeState, int width = 1)
        {
            SetButton(makeRoomUsableButton, activeState, width);
        }

        private void SetButton(RoomButton roomButton, bool activeState, int width)
        {
            roomButton.roomButtonRectTransform.anchoredPosition = roomButton.roomButtonPositionsList[width - 1].anchoredPosition;
            roomButton.roomButton.SetActive(activeState);
        }

        public bool GetMakeUsableButtonState()
        {
            return makeRoomUsableButton.roomButton.activeInHierarchy;
        }
        
        #endregion
    }
}
