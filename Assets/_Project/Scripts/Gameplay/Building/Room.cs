using System;
using System.Collections.Generic;
using Leguar.TotalJSON;
using UnityEngine;
using UnityEngine.Events;
using static _Project.Scripts.Gameplay.Building.Room.RoomTypeEnum;
using static _Project.Scripts.ScriptableObjects.EmployeeDict.EmployeeDictSo;
using Image = UnityEngine.UI.Image;

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

        public static string GetLegibleRoomName(RoomTypeEnum roomType)
        {
            return roomType switch
            {
                Bathroom => "Bathroom",
                CustomerSingle => "Single Room",
                CustomerDouble => "Double Room",
                Dining => "Dining Room",
                Arcade => "Arcade",
                Gym => "Gym",
                _ => throw new ArgumentOutOfRangeException(nameof(roomType), roomType, null)
            };
        }
        
        public static string GetLegibleEmployeeName(EmployeeTypesEnum employeeType)
        {
            return employeeType switch
            {
                EmployeeTypesEnum.Cleaner => "Cleaner",
                EmployeeTypesEnum.Cook => "Cook",
                EmployeeTypesEnum.GameTechnician => "Game Technician",
                EmployeeTypesEnum.GymCoach => "Gym Coach",
                _ => throw new ArgumentOutOfRangeException(nameof(employeeType), employeeType, null)
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

        [Space] 
        [Header("Unique Make Room Usable Icons")] 
        [SerializeField] private Image imageIcon;
        [Space]
        
        [SerializeField] private Sprite spriteClean;
        [SerializeField] private Sprite spriteCook;
        [SerializeField] private Sprite spriteArcade;
        [SerializeField] private Sprite spriteGym;
        

        #region Button Methods
        
        public void SetRemoveRoomButton(bool activeState, int width = 1)
        {
            SetButton(removeRoomButton, activeState, width);
        }
        
        public void SetMakeUsableButton(bool activeState, int width = 1)
        {
            SetMakeRoomUsableButtonImageByType();
            
            SetButton(makeRoomUsableButton, activeState, width);
        }
        
        private void SetButton(RoomButton roomButton, bool activeState, int width)
        {
            roomButton.roomButtonRectTransform.anchoredPosition = roomButton.roomButtonPositionsList[width - 1].anchoredPosition;
            roomButton.roomButton.SetActive(activeState);
        }

        private void SetMakeRoomUsableButtonImageByType()
        {
            switch (slot.roomType)
            {
                case CustomerSingle:
                case CustomerDouble:
                case Bathroom:
                    imageIcon.sprite = spriteClean;
                    break;
                case Dining:
                    imageIcon.sprite = spriteCook;
                    break;
                case Gym:
                    imageIcon.sprite = spriteGym;
                    break;
                case Arcade:
                    imageIcon.sprite = spriteArcade;
                    break;
            }
        }

        #endregion
    }
}
